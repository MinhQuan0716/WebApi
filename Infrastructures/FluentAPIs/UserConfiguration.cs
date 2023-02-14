using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.UserName).HasMaxLength(100);
            builder.Property(x => x.CreationDate).HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.LoginDate).HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.Role).HasDefaultValue("User");
            builder.Property(x => x.IsDeleted).HasDefaultValue("False");
        }
    }
}
