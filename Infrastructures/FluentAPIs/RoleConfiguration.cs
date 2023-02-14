using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.FluentAPIs
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new Role
            {
                RoleId = 1,
                RoleName = "SuperAdmin",
                ClassPermission = nameof(PermissionEnum.FullAccess),
                TrainingProgramPermission = nameof(PermissionEnum.FullAccess)
            ,
                LearningMaterial = nameof(PermissionEnum.FullAccess),
                SyllabusPermission = nameof(PermissionEnum.FullAccess),
                UserPermission = nameof(PermissionEnum.FullAccess)
            },
            new Role
            {
                RoleId = 2,
                RoleName = "ClassAdmin",
                ClassPermission = nameof(PermissionEnum.FullAccess),
                TrainingProgramPermission = nameof(PermissionEnum.FullAccess)
            ,
                LearningMaterial = nameof(PermissionEnum.FullAccess),
                SyllabusPermission = nameof(PermissionEnum.FullAccess),
                UserPermission = nameof(PermissionEnum.FullAccess)
            },
            new Role
            {
                RoleId = 3,
                RoleName = "Trainer",
                ClassPermission = nameof(PermissionEnum.FullAccess),
                TrainingProgramPermission = nameof(PermissionEnum.FullAccess)
            ,
                LearningMaterial = nameof(PermissionEnum.FullAccess),
                SyllabusPermission = nameof(PermissionEnum.FullAccess),
                UserPermission = nameof(PermissionEnum.FullAccess)
            },
            new Role
            {
                RoleId = 4,
                RoleName = "Student",
                ClassPermission = nameof(PermissionEnum.View),
                TrainingProgramPermission = nameof(PermissionEnum.View)
            ,
                LearningMaterial = nameof(PermissionEnum.View),
                SyllabusPermission = nameof(PermissionEnum.View),
                UserPermission = nameof(PermissionEnum.AccessDenied)
            }





            );
        }
    }
}
