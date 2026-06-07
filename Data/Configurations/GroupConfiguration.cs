using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalRChatApp.Models;

namespace SignalRChatApp.Data.Configurations
{
  public class GroupConfiguration : IEntityTypeConfiguration<Group>
  {
    public void Configure(EntityTypeBuilder<Group> builder)
    {
      builder.HasKey(x => x.Id);

      builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);

      builder.Property(x => x.OwnerId)
        .IsRequired();

      builder.HasOne(x => x.Owner)
        .WithMany(x => x.OwnedGroups)
        .HasForeignKey(x => x.OwnerId)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
