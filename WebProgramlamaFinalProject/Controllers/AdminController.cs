using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebProgramlamaFinalProject.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Dashboard()
		{
			return View();
		}


		// ================= Appointments ==================
		public IActionResult ManageAppointments()
		{
			return View();
		}

		//public IActionResult ApproveAppointment(int id) { ... }
		//public IActionResult RejectAppointment(int id) { ... }

		// ================== Trainers ==================
		public IActionResult ManageTrainers()
		{
			return View();
		}

		//public IActionResult CreateTrainer() { ... }
		//public IActionResult EditTrainer(int id) { ... }
		//public IActionResult DeleteTrainer(int id) { ... }

		// ================== Schedules ==================
		//public IActionResult TrainerSchedule(int trainerId) { ... }
		//public IActionResult SaveTrainerSchedule(...) { ... }

		// ================== Services ==================
		public IActionResult ManageServices()
		{
			return View();
		}

		//public IActionResult CreateService() { ... }
		//public IActionResult EditService(int id) { ... }
		//public IActionResult DeleteService(int id) { ... }



	}
}
