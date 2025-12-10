using System.Collections.Generic;

namespace WebProgramlamaFinalProject.Models.ViewModels
{
	public class TrainerFormViewModel
	{
		public Trainer Trainer { get; set; }
		public List<Service> Services { get; set; }
		public List<int> SelectedServiceIds { get; set; }

		public TrainerFormViewModel()
		{
			SelectedServiceIds = new List<int>();
			Services = new List<Service>();
		}

	}
}
