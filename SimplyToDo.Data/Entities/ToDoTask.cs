using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimplyToDo.Data.Entities;

internal class ToDoTask
{
    public int Id { get; set; }

    // The navigation property is nullable even though it's required as 
    // we may not always load the related list when querying.
    public int ToDoListId { get; set; }
    public ToDoList? ToDoList { get; set; }

    // A custom user task status
    public string? TaskStatus { get; set; }

    // A user settable background color in HTML Hex format
    public string? BackgroundColor { get; set; }

    // A user settable foreground color in HTML Hex format
    public string? ForegroundColor { get; set; }

    public bool IsCompleted { get; set; }

    // TODO: Should we limit the maximum length of a task description?
    public required string Description { get; set; }

    public DateOnly? DueByDate { get; set; }

    public TimeOnly? DueByTime { get; set; }
}

internal class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.ToTable(nameof(ToDoTask));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.TaskStatus)
            .IsRequired(false)
            .HasMaxLength(32);

        builder.Property(p => p.BackgroundColor)
            .IsRequired(false)
            .HasMaxLength(9);

        builder.Property(p => p.ForegroundColor)
            .IsRequired(false)
            .HasMaxLength(9);

        builder.Property(p => p.Description)
            .IsRequired(true);

        builder.Property(p => p.DueByDate)
            .IsRequired(false);

        builder.Property(p => p.DueByTime)
            .IsRequired(false);

        builder.HasOne(p => p.ToDoList)
            .WithMany(p => p.Tasks)
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ToDoListId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
    }
}