using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimplyToDo.Data.Entities;

internal class SharedToDoList
{
    public int Id { get; set; }

    // Navigation property is null even though it's required as it may not
    // include it when querying.
    public int ToDoListId { get; set; }
    public ToDoTask? ToDoList { get; set; }

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

internal class SharedToDoListConfiguration : IEntityTypeConfiguration<SharedToDoList>
{
    public void Configure(EntityTypeBuilder<SharedToDoList> builder)
    {
        builder.ToTable(nameof(SharedToDoList));

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.ToDoList)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ToDoListId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.SharedUser)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.SharedUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.ToDoListId)
            .IsUnique(false);

        builder.HasIndex(p => p.SharedUserId)
            .IsUnique(false);
    }
}
