using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaFinalProject.Data;
using WebProgramlamaFinalProject.Models;

namespace WebProgramlamaFinalProject.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		private readonly ApplicationDbContext _context;

		public AdminController(ApplicationDbContext context)
		{
			_context = context;
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





		// ================== Trainers ==================//
		// ================== Trainers ==================//
		// ================== Trainers ==================//
		public IActionResult ManageTrainers()
		{
			var trainers = _context.Trainers.ToList();
			return View(trainers);
		}


		[HttpGet]
		public IActionResult CreateTrainer()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateTrainer(Trainer model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			_context.Trainers.Add(model);
			_context.SaveChanges();

			return RedirectToAction("ManageTrainers");
		}

		public IActionResult DeleteTrainer(int id)
		{
			var trainer = _context.Trainers.FirstOrDefault(t => t.Id == id);

			if (trainer == null)
				return NotFound();

			// Hapus semua schedule dulu
			var schedules = _context.TrainerSchedules.Where(s => s.TrainerId == id).ToList();
			_context.TrainerSchedules.RemoveRange(schedules);

			// Baru hapus trainer
			_context.Trainers.Remove(trainer);
			_context.SaveChanges();

			return RedirectToAction("ManageTrainers");
		}


		// GET: /Admin/EditTrainer/5
		[HttpGet]
		public IActionResult EditTrainer(int id)
		{
			var trainer = _context.Trainers.FirstOrDefault(t => t.Id == id);
			if (trainer == null)
				return NotFound();

			return View(trainer);
		}

		// POST: /Admin/EditTrainer/5
		[HttpPost]
		public IActionResult EditTrainer(Trainer model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var trainer = _context.Trainers.FirstOrDefault(t => t.Id == model.Id);
			if (trainer == null)
				return NotFound();

			trainer.Name = model.Name;
			trainer.Specialization = model.Specialization;

			_context.SaveChanges();

			return RedirectToAction("ManageTrainers");
		}


		// GET: Set Schedule form
		[HttpGet]
		public IActionResult SetSchedule(int id)
		{
			var trainer = _context.Trainers.FirstOrDefault(t => t.Id == id);
			if (trainer == null)
				return NotFound();

			var schedules = _context.TrainerSchedules
				.Where(s => s.TrainerId == id)
				.ToList();

			ViewBag.TrainerName = trainer.Name;
			ViewBag.TrainerId = trainer.Id;

			return View(schedules);
		}


		// POST: Save new schedule
		[HttpPost]
		public IActionResult SetSchedule(int id, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
		{
			if (startTime >= endTime)
			{
				ModelState.AddModelError("", "Start time must be before End time");
				return RedirectToAction("SetSchedule", new { id });
			}

			var schedule = new TrainerSchedule
			{
				TrainerId = id,
				DayOfWeek = dayOfWeek,
				StartTime = startTime,
				EndTime = endTime
			};

			_context.TrainerSchedules.Add(schedule);
			_context.SaveChanges();

			return RedirectToAction("SetSchedule", new { id });
		}

		public IActionResult DeleteSchedule(int id, int trainerId)
		{
			var schedule = _context.TrainerSchedules.FirstOrDefault(s => s.Id == id);

			if (schedule == null)
				return NotFound();

			_context.TrainerSchedules.Remove(schedule);
			_context.SaveChanges();

			return RedirectToAction("SetSchedule", new { id = trainerId });

		}


		// ================== Trainers ==================//
		// ================== Trainers ==================//
		// ================== Trainers ==================//





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
