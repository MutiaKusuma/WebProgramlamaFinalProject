using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaFinalProject.Models
{
	public class Trainer
	{
		public Trainer()
		{
			Schedules = new List<TrainerSchedule>();
			TrainerServices = new List<TrainerService>();

		}
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		//public string Specialization { get; set; } //dihapus soalnya mau ganti kecheckbox

		// Navigasi ke jadwal
		public ICollection<TrainerSchedule> Schedules { get; set; }

		// MANY–TO–MANY
		public List<TrainerService> TrainerServices { get; set; } = new();


	}

}

