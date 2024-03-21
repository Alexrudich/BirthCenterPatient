using BirthCenterPatientAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BirthCenterPatientAPI.Data
{
    public class PatientContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientGender> PatientGenders { get; set; }
        public DbSet<PatientActivity> PatientActivities { get; set; }

        public PatientContext(DbContextOptions<PatientContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(p => p.Gender).HasConversion<string>();
            modelBuilder.Entity<PatientActivity>().HasKey(pa => pa.Id);
        }
    }
}
