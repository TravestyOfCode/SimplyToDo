using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SimplyToDo.Data.Entities;

namespace SimplyToDo.Data;

internal class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<ToDoList> ToDoLists { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }
}

internal class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        var connString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Unable to get connection string.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
