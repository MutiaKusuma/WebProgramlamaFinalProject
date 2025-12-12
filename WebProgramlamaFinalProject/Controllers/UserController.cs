using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgramlamaFinalProject.Data;

namespace WebProgramlamaFinalProject.Controllers
{
	[Authorize(Roles = "User")]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}


		public async Task<IActionResult> Dashboard()
		{
			var user = await _userManager.GetUserAsync(User);
			var userName = user?.Email ?? "User";

			ViewBag.UserName = userName;

			return View();
		}


		// ================= Make Appointment ==================//
		// ================= Make Appointment ==================//
		// ================= Make Appointment ==================//


		// STEP 1 - Choose Service
		[HttpGet]
		public IActionResult ChooseService()
		{
			var services = _context.Services.ToList();
			return View(services);
		}

		[HttpPost]
		public IActionResult ChooseService(int serviceId)
		{
			if (serviceId == 0)
				return RedirectToAction("ChooseService");

			return RedirectToAction("ChooseTrainer", new { serviceId = serviceId });
		}


		// STEP 2 - Choose Trainer
		[HttpGet]
		public IActionResult ChooseTrainer(int serviceId)
		{
			var service = _context.Services.Find(serviceId);

			if (service == null)
				return RedirectToAction("ChooseService");

			var trainers = _context.TrainerServices
								   .Include(ts => ts.Trainer)
								   .Where(ts => ts.ServiceId == serviceId)
								   .Select(ts => ts.Trainer)
								   .ToList();

			ViewBag.Service = service;
			return View(trainers);
		}

		[HttpPost]
		public IActionResult ChooseTrainer(int serviceId, string trainerId)
		{
			if (string.IsNullOrEmpty(trainerId))
				return RedirectToAction("ChooseTrainer", new { serviceId });

			return RedirectToAction("ChooseDate", new { serviceId, trainerId });
		}

	}
	/*
	// STEP 2: Pilih Trainer
	[HttpGet]
	public IActionResult MakeAppointmentStep2(int serviceId)
	{
		var service = _context.Services.Find(serviceId);
		var trainers = _context.TrainerServices
							   .Where(ts => ts.ServiceId == serviceId)
							   .Select(ts => ts.Trainer)
							   .ToList();

		var vm = new MakeAppointmentStep2ViewModel
		{
			SelectedService = service,
			AvailableTrainers = trainers
		};
		return View(vm);
	}

	[HttpPost]
	public IActionResult MakeAppointmentStep2(MakeAppointmentStep2ViewModel model)
	{
		if (model.SelectedTrainerId == null)
		{
			model.AvailableTrainers = _context.TrainerServices
				.Where(ts => ts.ServiceId == model.SelectedService.Id)
				.Select(ts => ts.Trainer).ToList();
			return View(model);
		}

		return RedirectToAction("MakeAppointmentStep3", new { trainerId = model.SelectedTrainerId, serviceId = model.SelectedService.Id });
	}

	// STEP 3: Pilih Hari & Slot
	[HttpGet]
	public IActionResult MakeAppointmentStep3(int serviceId, int trainerId)
	{
		// generate slot logic nanti
	}

	// STEP 4: Review & Submit
	[HttpPost]
	public IActionResult SubmitAppointment(ReviewAppointmentViewModel model)
	{
		// create appointment record
	}




*/

	// ================= Make Appointments ==================//
	// ================= Make Appointments ==================//
	// ================= Make Appointments ==================//

}
