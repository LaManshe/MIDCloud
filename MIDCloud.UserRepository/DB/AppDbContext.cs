using Microsoft.EntityFrameworkCore;
using MIDCloud.UserRepository.Models;

namespace MIDCloud.UserRepository.DB;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}