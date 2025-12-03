using System;
using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaFinalProject.Models
{
	public class TrainerSchedule
	{
		public int Id { get; set; }

		[Required]
		public int TrainerId { get; set; }
		public Trainer Trainer { get; set; }

		[Required]
		public DayOfWeek DayOfWeek { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }
	}
}
