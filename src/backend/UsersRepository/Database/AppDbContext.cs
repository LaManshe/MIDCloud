using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MIDCloud.GlobalInterfaces.Users;
using MIDCloud.GlobalInterfaces.Models;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(field => field.PermissionsToFolder)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v));
        }
    }
}
