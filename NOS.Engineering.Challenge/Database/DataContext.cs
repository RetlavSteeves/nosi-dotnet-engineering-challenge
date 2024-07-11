using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NOS.Engineering.Challenge.Models;

namespace NOS.Engineering.Challenge.Database;



public class DataContext : DbContext
{
    private readonly IConfiguration _config;

     public DataContext()
     {
      
     }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));

    }

    public DbSet<ContentDto> Content {get;set;}
}