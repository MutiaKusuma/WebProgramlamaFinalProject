using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json;
using WebProgramlamaFinalProject.Data;
using WebProgramlamaFinalProject.Models;
using WebProgramlamaFinalProject.Models.ViewModels;
using WebProgramlamaFinalProject.Services;
using System.Text.Json;




namespace WebProgramlamaFinalProject.Controllers
{
	[Authorize(Roles = "User")]

	public class UserController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;


		public UserController(
		ApplicationDbContext context,
		UserManager<IdentityUser> userManager,
		IConfiguration configuration)
		{
			_context = context;
			_userManager = userManager;
			_configuration = configuration;
		}



		public async Task<IActionResult> Dashboard()
		{
			var user = await _userManager.GetUserAsync(User);
			var userName = user?.Email ?? "User";

			ViewBag.UserName = userName;

			return View();
		}



		// ================= MY Appointments ==================//
		// ================= My Appointments ==================//
		// ================= MY Appointments ==================//

		[HttpGet]
		public async Task<IActionResult> MyAppointments()
		{
			var user = await _userManager.GetUserAsync(User);

			var appointments = _context.Appointments
				.Include(a => a.Trainer)
				.Include(a => a.Service)
				.Where(a => a.UserId == user.Id)
				.OrderByDescending(a => a.StartTime)
				.ToList();

			ViewBag.UserName = user.Email; // atau user.UserName

			return View(appointments);
		}


		// ================= MY Appointments ==================//
		// ================= My Appointments ==================//
		// ================= MY Appointments ==================//

		//--------------------------------------------------------------------------------------//

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
		public IActionResult ChooseTrainer(int serviceId, int trainerId)
		{
			var schedules = _context.TrainerSchedules
				.Where(s => s.TrainerId == trainerId)
				.ToList();

			if (!schedules.Any())
				return Content("Trainer tidak punya jadwal");

			DateTime date = DateTime.Today;

			// ⬇️ lompat hari sampai ketemu schedule
			while (!schedules.Any(s => s.DayOfWeek == date.DayOfWeek))
			{
				date = date.AddDays(1);
			}

			return RedirectToAction("ChooseSlot", new { serviceId, trainerId });


		}

		// STEP 3 - Choose Date
		[HttpGet]
		public IActionResult ChooseSlot(int trainerId, int serviceId)
		{
			var service = _context.Services.First(s => s.Id == serviceId);

			var schedules = _context.TrainerSchedules
				.Where(s => s.TrainerId == trainerId)
				.ToList();

			var model = new ChooseSlotViewModel
			{
				TrainerId = trainerId,
				ServiceId = serviceId,
				ServiceDuration = service.DurationInMinutes
			};

			DateTime startDate = DateTime.Today;

			for (int i = 0; i < 14; i++) // 2 minggu
			{
				DateTime date = startDate.AddDays(i);

				var schedule = schedules
					.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);

				if (schedule == null)
					continue;

				var group = new DateSlotGroupViewModel
				{
					Date = date
				};

				DateTime current = date.Date.Add(schedule.StartTime);
				DateTime end = date.Date.Add(schedule.EndTime);

				while (current.AddMinutes(service.DurationInMinutes) <= end)
				{
					group.Slots.Add(new SlotItemViewModel
					{
						StartTime = current,
						EndTime = current.AddMinutes(service.DurationInMinutes)
					});

					current = current.AddMinutes(service.DurationInMinutes);
				}

				if (group.Slots.Any())
					model.DateGroups.Add(group);
			}

			return View(model);
		}

		[HttpPost]
		public IActionResult ChooseSlot(ChooseSlotViewModel model)
		{
			if (!model.SelectedSlot.HasValue)
			{
				model.ErrorMessage = "Please select a time slot.";
				return ReloadChooseSlot(model);
			}

			DateTime slotStart = model.SelectedSlot.Value;
			DateTime slotEnd = slotStart.AddMinutes(model.ServiceDuration);

			bool isTaken = _context.Appointments.Any(a =>
				a.TrainerId == model.TrainerId &&
				slotStart < a.EndTime &&
				slotEnd > a.StartTime
			);

			if (isTaken)
			{
				model.ErrorMessage = "⚠️ This time is UNAVAILABLE, please choose another";
				return ReloadChooseSlot(model);
			}

			TempData["TrainerId"] = model.TrainerId;
			TempData["ServiceId"] = model.ServiceId;
			TempData["StartTime"] = slotStart;
			TempData["EndTime"] = slotEnd;

			return RedirectToAction("ReviewAppointment");
		}

		private IActionResult ReloadChooseSlot(ChooseSlotViewModel model)
		{
			var service = _context.Services.First(s => s.Id == model.ServiceId);
			var schedules = _context.TrainerSchedules
				.Where(s => s.TrainerId == model.TrainerId)
				.ToList();

			model.DateGroups.Clear();

			DateTime startDate = DateTime.Today;

			for (int i = 0; i < 14; i++)
			{
				DateTime date = startDate.AddDays(i);

				var schedule = schedules
					.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);

				if (schedule == null)
					continue;

				var group = new DateSlotGroupViewModel { Date = date };

				DateTime current = date.Date.Add(schedule.StartTime);
				DateTime end = date.Date.Add(schedule.EndTime);

				while (current.AddMinutes(service.DurationInMinutes) <= end)
				{
					group.Slots.Add(new SlotItemViewModel
					{
						StartTime = current,
						EndTime = current.AddMinutes(service.DurationInMinutes)
					});

					current = current.AddMinutes(service.DurationInMinutes);
				}

				if (group.Slots.Any())
					model.DateGroups.Add(group);
			}

			return View("ChooseSlot", model);
		}



		[HttpGet]
		public IActionResult ReviewAppointment()
		{
			if (TempData["TrainerId"] == null)
				return RedirectToAction("Dashboard");

			int trainerId = (int)TempData["TrainerId"];
			int serviceId = (int)TempData["ServiceId"];
			DateTime startTime = (DateTime)TempData["StartTime"];
			DateTime endTime = (DateTime)TempData["EndTime"];

			TempData.Keep(); // ⬅️ penting biar POST bisa pakai lagi

			var trainer = _context.Trainers.Find(trainerId);
			var service = _context.Services.Find(serviceId);

			var model = new ReviewAppointmentViewModel
			{
				TrainerId = trainerId,
				ServiceId = serviceId,
				TrainerName = trainer.Name,
				ServiceName = service.Name,
				StartTime = startTime,
				EndTime = endTime
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> SubmitAppointment()
		{
			var user = await _userManager.GetUserAsync(User);

			if (TempData["TrainerId"] == null)
				return RedirectToAction("Dashboard");

			var appointment = new Appointment
			{
				UserId = user.Id,
				TrainerId = (int)TempData["TrainerId"],
				ServiceId = (int)TempData["ServiceId"],
				StartTime = (DateTime)TempData["StartTime"],
				EndTime = (DateTime)TempData["EndTime"],
				Status = "Pending"
			};

			_context.Appointments.Add(appointment);
			await _context.SaveChangesAsync();

			return RedirectToAction("AppointmentSuccess");
		}


		[HttpGet]
		public IActionResult AppointmentSuccess()
		{
			return View();
		}



		// ================= Make Appointments ==================//
		// ================= Make Appointments ==================//
		// ================= Make Appointments ==================//


		//--------------------------------------------------------------------------------------//	



		// ================= Ai Feature ==================//
		// ================= Ai Feature ==================//
		// ================= Ai Feature ==================//


		[HttpGet]
		public IActionResult AiFeature()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> AiFeature(AiFitnessInputViewModel model)
		{
			// 1️⃣ HITUNG BMI (LOGIKA DASAR)
			double heightM = model.HeightCm / 100.0;
			double bmi = model.WeightKg / (heightM * heightM);

			string category =
				bmi < 18.5 ? "Underweight" :
				bmi < 25 ? "Normal" :
				bmi < 30 ? "Overweight" :
				"Obese";

			int dailyCalories = category switch
			{
				"Underweight" => 2500,
				"Normal" => 2200,
				"Overweight" => 1800,
				_ => 1600
			};

			// 2️⃣ CALL OPENAI
			var apiKey = _configuration["OpenAI:ApiKey"];

			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

			var prompt = $"User BMI: {bmi:F1}, category: {category}, suggest diet plan.";

			var body = new
			{
				model = "gpt-4o-mini",
				messages = new[]
				{
			new { role = "user", content = prompt }
		}
			};

			var response = await client.PostAsJsonAsync(
				"https://api.openai.com/v1/chat/completions", body);

			//var json = await response.Content.ReadFromJsonAsync<dynamic>();
			//string aiText = json.choices[0].message.content;

			var jsonString = await response.Content.ReadAsStringAsync();
			var jsonDoc = JsonDocument.Parse(jsonString);

			string aiText =
				jsonDoc.RootElement
					.GetProperty("choices")[0]
					.GetProperty("message")
					.GetProperty("content")
					.GetString();


			// 3️⃣ KIRIM KE RESULT VIEW
			var result = new AiFitnessResultViewModel
			{
				BMI = Math.Round(bmi, 2),
				Category = category,
				DailyCalories = dailyCalories,
				DietRecommendation = aiText
			};

			return View("AiResult", result);
		}





		// ================= Ai Feature ==================//
		// ================= Ai Feature ==================//
		// ================= Ai Feature ==================//
	}


}