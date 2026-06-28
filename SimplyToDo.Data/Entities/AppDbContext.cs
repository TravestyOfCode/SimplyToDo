using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimplyToDo.Data.Entities;

internal class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<ToDoList> ToDoLists { get; set; }
    public DbSet<ToDoTask> ToDoTasks { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}

internal class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var path = Directory.GetCurrentDirectory();

        var config = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Unable to get connection string.");

        var dbOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString);

        return new AppDbContext(dbOptionsBuilder.Options);
    }
}


