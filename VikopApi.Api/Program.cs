using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using VikopApi.Api.Infrastructure.BackgroundServices;
using VikopApi.Application.Models.Auth.Commands;
using VikopApi.Database;
using VikopApi.Domain.Models;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration["DefaultConnection"]));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
}).
AddEntityFrameworkStores<AppDbContext>().
AddDefaultTokenProviders();

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]);
    var key = new SymmetricSecurityKey(bytes);

    config.SaveToken = true;
    config.RequireHttpsMetadata = false;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = key,
        ValidIssuer = builder.Configuration["Auth:Issuer"],
        ValidAudience = builder.Configuration["Auth:Audience"],
    };
});

builder.Services.AddApplicationServices();
builder.Services.AddMediatR(typeof(LoginCommand));
builder.Services.AddSingleton<RankUpdater>();
builder.Services.AddHostedService<RankUpdater>();

builder.Services.AddControllers();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v2",
        Title = "Vikop API",
        Description = "API trying to mirror wykop.pl"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("Moderator", policy => policy.RequireClaim("Role", "Moderator"));

    options.AddPolicy("Moderator", policy =>
        policy.RequireAssertion(context =>
        context.User.HasClaim("Role", "Moderator")
        || context.User.HasClaim("Role", "Admin")));
});

var app = builder.Build();

try
{
    // Creating admin user if one does not exist
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        context.Database.EnsureCreated();

        var createAdmin = !context.UserClaims.Any(x => x.ClaimValue == "Admin");

        if (createAdmin)
        {
            Console.WriteLine("Admin user does not exist!");
            Console.WriteLine("Do you want to create it? y/n");
            var response = Console.ReadLine();
            createAdmin = response == "y" || response == "Y";
        }

        if (createAdmin)
        {
            Console.WriteLine();
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            var admin = new ApplicationUser
            {
                UserName = username,
                Email = email,
                ProfilePicture = builder.Configuration["Image:Placeholder"]
            };

            userManager.CreateAsync(admin, password).GetAwaiter().GetResult();

            var adminClaim = new Claim("Role", "Admin");

            userManager.AddClaimAsync(admin, adminClaim).GetAwaiter().GetResult();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
