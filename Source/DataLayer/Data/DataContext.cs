using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }
}