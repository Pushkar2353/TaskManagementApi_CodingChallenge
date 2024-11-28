using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagementApi_CodingChallenge.Models;

namespace TaskManagementApi_CodingChallenge.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext() { }
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) 
        { 

        }

        public DbSet<TaskModel> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.HasKey(t => t.TaskId);
                entity.Property(t => t.TaskId)
                      .ValueGeneratedOnAdd();

                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(t => t.Description)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(t => t.DueDate)
                      .IsRequired();

                entity.Property(t => t.Priority)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(t => t.Status)
                      .IsRequired()
                      .HasMaxLength(30);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var configSection = configBuilder.GetSection("ConnectionStrings");
            var conStr = configSection["conStr"] ?? null;

            optionsBuilder.UseSqlServer(conStr);

        }
    }
}
