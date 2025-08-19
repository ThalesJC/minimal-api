using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Infrastructure.DB;

class DBContext(IConfiguration configurationAppSettings) : DbContext
{

  private readonly IConfiguration _configurationAppSettings = configurationAppSettings;

  public DbSet<Admin> Administrators { get; set; } = default!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Admin>().HasData(
      new Admin { Email = "admi@email.com.br", Password="S3nh@ F0rt&", Role="admin"}
    );
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      var stringConection = _configurationAppSettings.GetConnectionString("mySql")?.ToString();
      if (!string.IsNullOrEmpty(stringConection))
        optionsBuilder.UseMySql(stringConection, ServerVersion.AutoDetect(stringConection));

    }
  }
}
