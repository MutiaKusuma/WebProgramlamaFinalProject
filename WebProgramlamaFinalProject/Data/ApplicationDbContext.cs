using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProgramlamaFinalProject.Models;

namespace WebProgramlamaFinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		public DbSet<Service> Services { get; set; }
		public DbSet<Trainer> Trainers { get; set; }
		public DbSet<TrainerSchedule> TrainerSchedules { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
	}
}
