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
		public DbSet<TrainerService> TrainerServices { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<TrainerService>()
				.HasKey(ts => new { ts.TrainerId, ts.ServiceId });

			modelBuilder.Entity<TrainerService>()
				.HasOne(ts => ts.Trainer)
				.WithMany(t => t.TrainerServices)
				.HasForeignKey(ts => ts.TrainerId);

			modelBuilder.Entity<TrainerService>()
				.HasOne(ts => ts.Service)
				.WithMany(s => s.TrainerServices)
				.HasForeignKey(ts => ts.ServiceId);
		}


	}
}
