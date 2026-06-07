using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalRChatApp.Models;

namespace SignalRChatApp.Data.Configurations
{
  public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
  {
    public void Configure(EntityTypeBuilder<UserConnection> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.ConnectionId).IsRequired();

      builder.HasIndex(x => x.ConnectionId).IsUnique();

      builder.HasOne(x => x.User)
             .WithMany(u => u.UserConnections)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
