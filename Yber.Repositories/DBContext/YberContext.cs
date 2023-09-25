using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yber.Repositories.Entities;

namespace Yber.Repositories.DBContext;

public class YberContext : DbContext
{
    public DbSet<Uber_Students> Uber_Students { get; set; }
    public DbSet<Uber_Requests> Uber_Requests { get; set; }
    public DbSet<Uber_Cities> Uber_Cities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO SQL CONNECTION STRING!
        var cnString = "server=10.131.15.57;user=program;password=SuperSecretPassword1337;database=UBER";
        // var cnString = "server=10.131.15.57;user=program;password=SuperSecretPassword1337;database=UBER";
        var serverVersion = new MySqlServerVersion(new Version(10,9,0));
        optionsBuilder.UseMySql(cnString, serverVersion)
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Uber_Students>()
            .HasOne(student => student.City)
            .WithMany(cities => cities.Student)
            .HasForeignKey(s => s.Zipcode);
        
        modelBuilder.Entity<Uber_Cities>()
            .HasKey(c => c.Zipcode);

        modelBuilder.Entity<Uber_Requests>()
        .HasKey(uR => new { uR.RequesteeID, uR.RequesterID });

        modelBuilder.Entity<Uber_Students>()
            .HasMany(student => student.Requests)
            .WithOne(requests => requests.Requester)
            .HasForeignKey(r => r.RequesterID);
    }
}
