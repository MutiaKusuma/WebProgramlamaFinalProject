using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaFinalProject.Data;
using WebProgramlamaFinalProject.Models;
using WebProgramlamaFinalProject.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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


		// ================= Appointments ==================//
		// ================= Appointments ==================//
		// ================= Appointments ==================//

		[HttpGet]
		public IActionResult ManageAppointments()
		{
			var appointments = _context.Appointments
				.Include(a => a.User)
				.Include(a => a.Trainer)
				.Include(a => a.Service)
				.OrderBy(a => a.StartTime)
				.ToList();

			return View(appointments);
		}


		[HttpPost]
		public IActionResult UpdateAppointmentStatus(int id, string status)
		{
			var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

			if (appointment == null)
				return NotFound();

			appointment.Status = status;
			_context.SaveChanges();

			return RedirectToAction("ManageAppointments");
		}

		// ================= Appointments ==================//
		// ================= Appointments ==================//
		// ================= Appointments ==================//


		/*----------------------------------------------------------------------*/


		// ================== Trainers ==================//
		// ================== Trainers ==================//
		// ================== Trainers ==================//


		public IActionResult ManageTrainers()
		{
			var trainers = _context.Trainers
				.Include(t => t.TrainerServices)       // load relasi many-to-many
					.ThenInclude(ts => ts.Service)     // load Service dari relasi
				.ToList();

			return View(trainers);
		}

		[HttpGet]
		public IActionResult CreateTrainer()
		{
			var vm = new TrainerFormViewModel
			{
				Trainer = new Trainer(),
				Services = _context.Services.ToList(),
				SelectedServiceIds = new List<int>()
			};

			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateTrainer([FromForm] TrainerFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				// reload services supaya checkbox tidak hilang
				model.Services = _context.Services.ToList();
				return View(model);
			}

			// 1️⃣ Simpan trainer dulu
			_context.Trainers.Add(model.Trainer);
			_context.SaveChanges();

			// 2️⃣ Simpan relasi many-to-many
			foreach (var serviceId in model.SelectedServiceIds)
			{
				var ts = new TrainerService
				{
					TrainerId = model.Trainer.Id,
					ServiceId = serviceId
				};
				_context.TrainerServices.Add(ts);
			}

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
			var trainer = _context.Trainers
				.Include(t => t.TrainerServices)
				.FirstOrDefault(t => t.Id == id);

			if (trainer == null)
				return NotFound();

			var vm = new TrainerFormViewModel
			{
				Trainer = trainer,
				Services = _context.Services.ToList(),
				SelectedServiceIds = trainer.TrainerServices.Select(ts => ts.ServiceId).ToList()
			};

			return View(vm);
		}

		// POST: /Admin/EditTrainer/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditTrainer([FromForm] TrainerFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Services = _context.Services.ToList();
				return View(model);
			}

			var trainer = _context.Trainers
				.Include(t => t.TrainerServices)
				.FirstOrDefault(t => t.Id == model.Trainer.Id);

			if (trainer == null)
				return NotFound();

			// Update nama trainer
			trainer.Name = model.Trainer.Name;

			// Hapus relasi many-to-many lama
			_context.TrainerServices.RemoveRange(trainer.TrainerServices);

			// Tambahkan relasi baru
			foreach (var serviceId in model.SelectedServiceIds)
			{
				_context.TrainerServices.Add(new TrainerService
				{
					TrainerId = trainer.Id,
					ServiceId = serviceId
				});
			}

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


		/*----------------------------------------------------------------------*/


		// ================== Services ==================//
		// ================== Services ==================//
		// ================== Services ==================//
		public IActionResult ManageServices()
		{
			var services = _context.Services.ToList();
			return View(services);
		}


		// GET
		[HttpGet]
		public IActionResult CreateService()
		{
			return View();
		}

		// POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateService(Service model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			_context.Services.Add(model);
			_context.SaveChanges();

			return RedirectToAction("ManageServices");
		}


		// GET: /Admin/EditService
		[HttpGet]
		public IActionResult EditService(int id)
		{
			var service = _context.Services.FirstOrDefault(s => s.Id == id);
			if (service == null)
				return NotFound();

			return View(service);
		}


		// POST: /Admin/EditService
		[HttpPost]
		public IActionResult EditService(Service model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var service = _context.Services.FirstOrDefault(s => s.Id == model.Id);
			if (service == null)
				return NotFound();

			service.Name = model.Name;
			service.DurationInMinutes = model.DurationInMinutes;
			service.Price = model.Price;

			_context.SaveChanges();

			return RedirectToAction("ManageServices");
		}


		public IActionResult DeleteService(int id)
		{
			var service = _context.Services.Find(id);
			if (service == null) return NotFound();

			_context.Services.Remove(service);
			_context.SaveChanges();

			return RedirectToAction("ManageServices");
		}


		// ================== Services ==================//
		// ================== Services ==================//
		// ================== Services ==================//



	}
}
