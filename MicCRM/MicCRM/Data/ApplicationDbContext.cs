using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicCRM.Models;
using MicCRM.Data.Entities;

namespace MicCRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudentLessons>()
                .HasKey(sl => new { sl.StudentId, sl.LessonId });

            builder.Entity<StudentLessons>()
                .HasOne(sl => sl.Student)
                .WithMany(s => s.StudentLessons)
                .HasForeignKey(sl => sl.StudentId);

            builder.Entity<StudentLessons>()
                .HasOne(sl => sl.Lesson)
                .WithMany(s => s.LessonStudents)
                .HasForeignKey(sl => sl.LessonId);
        }

        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Technology> Technologies { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }

    }
}
