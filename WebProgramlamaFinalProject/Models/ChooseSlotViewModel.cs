using System;
using System.Collections.Generic;

namespace WebProgramlamaFinalProject.Models.ViewModels
{
	public class ChooseSlotViewModel
	{
		public int TrainerId { get; set; }
		public int ServiceId { get; set; }
		public DateTime SelectedDate { get; set; }

		public int ServiceDuration { get; set; }

		public List<SlotItemViewModel> Slots { get; set; }

		public DateTime? SelectedSlot { get; set; }

		public string? ErrorMessage { get; set; }
		public List<DateSlotGroupViewModel> DateGroups { get; set; } = new();
	}

}
