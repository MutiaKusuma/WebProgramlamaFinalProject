namespace WebProgramlamaFinalProject.Models
{
	public class ReviewAppointmentViewModel
	{
		public int TrainerId { get; set; }
		public int ServiceId { get; set; }

		public string TrainerName { get; set; }
		public string ServiceName { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
