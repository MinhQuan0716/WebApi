using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.FluentAPIs
{
    public class TrainingClassConfiguration : IEntityTypeConfiguration<TrainingClass>
    {
        public void Configure(EntityTypeBuilder<TrainingClass> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.CreationDate).HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.IsDeleted).HasDefaultValue("false");
            builder.HasOne(x => x.Location).WithMany(x => x.TrainingClasses).HasForeignKey(x => x.LocationID);
            builder.HasMany(x => x.Attendances).WithOne(x => x.TrainingClass).HasForeignKey(x => x.TrainingClassId);

            builder.HasOne(c => c.TrainingProgram)
            .WithMany(x => x.TrainingClasses)
            .HasForeignKey(c => c.TrainingProgramId);
        }
    }
}
