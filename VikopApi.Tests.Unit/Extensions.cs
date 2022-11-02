using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using VikopApi.Database;

namespace VikopApi.Tests.Unit
{
    public static class Extensions
    {
        public static AppDbContext GetAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new AppDbContext(options);
        }

        public static void AddContent<T>(this AppDbContext context, List<T> content) where T : class
        {
            context.AddRange(content.ToList<object>());
            context.SaveChanges();
        }
    }
}
