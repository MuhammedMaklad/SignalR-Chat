using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalRChatApp.Models;

namespace SignalRChatApp.Data.Configurations
{
  public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
  {
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
      builder.HasKey(x=> new {x.UserId, x.GroupId });

      builder.HasOne(x => x.User)
        .WithMany(x => x.UserGroups)
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(x => x.Group)
        .WithMany(x => x.GroupUsers)
        .HasForeignKey(x => x.GroupId)
        .OnDelete(DeleteBehavior.Cascade); 
    }
  }
}
