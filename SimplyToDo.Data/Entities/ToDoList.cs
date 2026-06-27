using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimplyToDo.Data.Entities;

internal class ToDoList
{
    public int Id { get; set; }

    // The navigation property is nullable even though it's required as 
    // we may not always load the related list when querying.
    public required string OwnerId { get; set; }
    public AppUser? Owner { get; set; }

    // A user settable background color in HTML Hex format
    public string? BackgroundColor { get; set; }

    // A user settable foreground color in HTML Hex format
    public string? ForegroundColor { get; set; }

    // Used to allow the user to hide fully completed lists.
    public bool IsCompleted { get; set; }

    // Nullable as the user may have not added any tasks to the list
    public List<ToDoTask>? Tasks { get; set; }
}

internal class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
{
    public void Configure(EntityTypeBuilder<ToDoList> builder)
    {
        builder.ToTable(nameof(ToDoList));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.BackgroundColor)
            .IsRequired(false)
            .HasMaxLength(9);

        builder.Property(p => p.ForegroundColor)
            .IsRequired(false)
            .HasMaxLength(9);

        builder.HasOne(p => p.Owner)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.OwnerId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
