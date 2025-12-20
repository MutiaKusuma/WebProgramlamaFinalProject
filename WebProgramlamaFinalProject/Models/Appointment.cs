using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebProgramlamaFinalProject.Models
{
	public class Appointment
	{

		public int Id { get; set; }

		[Required]
		public string UserId { get; set; }
		public IdentityUser User { get; set; }

		[Required]
		public int TrainerId { get; set; }
		public Trainer Trainer { get; set; }

		[Required]
		public int ServiceId { get; set; }
		public Service Service { get; set; }

		[Required]
		public DateTime StartTime { get; set; }

		[Required]
		public DateTime EndTime { get; set; }

		[Required]
		public string Status { get; set; } 


	}
}
