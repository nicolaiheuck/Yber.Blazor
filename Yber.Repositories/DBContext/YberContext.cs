using Microsoft.EntityFrameworkCore;
using Yber.Repositories.Entities;

namespace Yber.Repositories.DBContext;

public class YberContext : DbContext
{
    public DbSet<Uber_Students> Uber_Stutents { get; set; }
    public DbSet<Uber_Requests> Uber_Requests { get; set; }
    public DbSet<Uber_Cities> Uber_Cities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var cnString = "server=10.131.15.57;user id=program;password=SuperSecretPassword1337;database=UBER";
        optionsBuilder.UseMySQL(cnString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Uber_Students>()
            .HasOne(student => student.City)
            .WithMany(cities => cities.Student);

        modelBuilder.Entity<Uber_Requests>()
            .HasKey(uR => new { uR.Requestee, uR.Requester });
    }
}
