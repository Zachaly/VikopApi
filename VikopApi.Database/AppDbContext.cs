﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Finding> Findings { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}