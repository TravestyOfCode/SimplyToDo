using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimplyToDo.Data.Entities;

internal class SharedToDoTask
{
    public int Id { get; set; }

    // Navigation property is null even though it's required as it may not
    // include it when querying.
    public int ToDoTaskId { get; set; }
    public ToDoTask? ToDoTask { get; set; }

    // The user that this task is shared with. Navigation property null as
    // it may not be included when querying.
    public required string SharedUserId { get; set; }
    public AppUser? SharedUser { get; set; }

    // Indicates if the shared user is allowed to complete the task.
    public bool CanComplete { get; set; }

    // Indicates if the shared user is allowed to edit other fields besides
    // the IsComplete property.
    public bool CanEdit { get; set; }

    // Indicates if the shared user is allowed to delete the task.
    public bool CanDelete { get; set; }
}

internal class SharedToDoTaskConfiguration : IEntityTypeConfiguration<SharedToDoTask>
{
    public void Configure(EntityTypeBuilder<SharedToDoTask> builder)
    {
        builder.ToTable(nameof(SharedToDoTask));

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.ToDoTask)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ToDoTaskId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.SharedUser)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.SharedUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.ToDoTaskId)
            .IsUnique(false);

        builder.HasIndex(p => p.SharedUserId)
            .IsUnique(false);
    }
}
