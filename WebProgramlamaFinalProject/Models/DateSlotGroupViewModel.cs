namespace WebProgramlamaFinalProject.Models
{
	public class DateSlotGroupViewModel
	{
		public DateTime Date { get; set; }
		public string DayName => Date.DayOfWeek.ToString();
		public List<SlotItemViewModel> Slots { get; set; } = new();
	}
}
