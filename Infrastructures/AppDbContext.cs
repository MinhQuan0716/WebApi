using Application.Interfaces;
using Domain.Entities;
using Infrastructures.FluentAPIs;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

        #region DBSet
        public DbSet<User> Users { get; set; }
        public DbSet<Syllabus> Syllabuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DetailUnitLecture> DetailUnitLecture { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Lecture> Lectures { get;set; }
        public DbSet<TrainingMaterial> TrainingMaterials { get; set; }
        public DbSet<TrainingClass> TrainingClasses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<DetailTrainingClassParticipate> DetailTrainingClassParticipates { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Applications> Applications { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<DetailTrainingProgramSyllabus> detailTrainingProgramSyllabuses { get; set; }
              
        public DbSet<Feedback> Feedbakcks { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new RoleConfiguration().Configure(modelBuilder.Entity<Role>());
            new SyllabusConfiguration().Configure(modelBuilder.Entity<Syllabus>());
            new TrainingMaterialsConfiguration().Configure(modelBuilder.Entity<TrainingMaterial>());
            new TrainingClassConfiguration().Configure(modelBuilder.Entity<TrainingClass>());
            new LocationConfiguration().Configure(modelBuilder.Entity<Location>());
            new DetailTrainingClassParticipateConfiguration().Configure(modelBuilder.Entity<DetailTrainingClassParticipate>());
            new ApplicationsConfiguration().Configure(modelBuilder.Entity<Applications>());
            new AttendanceConfiguration().Configure(modelBuilder.Entity<Attendance>());
            new TrainingProgramConfiguration().Configure(modelBuilder.Entity<TrainingProgram>());
            new DetailTrainingProgramSyllabusConfiguration().Configure(modelBuilder.Entity<DetailTrainingProgramSyllabus>());
            new FeedbackConfiguration().Configure(modelBuilder.Entity<Feedback>());
        }

    }
}
