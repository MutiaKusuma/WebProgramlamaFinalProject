using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaFinalProject.Models
{
	public class Trainer
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Specialization { get; set; }

		// Navigasi ke jadwal
		public ICollection<TrainerSchedule> Schedules { get; set; }
	}
}
