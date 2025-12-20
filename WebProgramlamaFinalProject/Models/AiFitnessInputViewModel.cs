using System.ComponentModel.DataAnnotations;


namespace WebProgramlamaFinalProject.Models
{
	public class AiFitnessInputViewModel
	{
		[Required]
		[Range(100, 250)]
		public int HeightCm { get; set; }


		[Required]
		[Range(30, 200)]
		public int WeightKg { get; set; }

		public IFormFile? BodyImage { get; set; }
		public string? TargetGoal { get; set; }
	}
}
