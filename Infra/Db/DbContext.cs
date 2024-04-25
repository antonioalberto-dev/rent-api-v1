using Microsoft.EntityFrameworkCore;
using RentApi.Domain.Entities;

namespace RentApi.Infra.Db;

public class DbContextInfra : DbContext
{
    private readonly IConfiguration _configurationAppSettings;
    public DbContextInfra(IConfiguration configurationAppSettings)
    {
        _configurationAppSettings = configurationAppSettings;
    }
    public DbSet<Admin> AdminUsers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().HasData(
            new Admin{
                Id = 1,
                Email = "adm@test.com",
                Password = "123456",
                Profile = "Adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var conectionString = _configurationAppSettings.GetConnectionString("mysql").ToString();
            if (!string.IsNullOrEmpty(conectionString))
            {
                optionsBuilder.UseMySql(
                    conectionString,
                    ServerVersion.AutoDetect(conectionString)
                );
            }
        }
    }
}