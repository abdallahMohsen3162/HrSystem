using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<AttendanceTable> AttendanceTables { get; set; }

        public DbSet<PrivateHoliday> PrivateHolidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneralSettings>()
                .Property(gs => gs.WeeklyHolidays)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
