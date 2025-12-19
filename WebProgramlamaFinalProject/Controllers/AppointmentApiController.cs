using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgramlamaFinalProject.Data;

namespace WebProgramlamaFinalProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AppointmentsApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public AppointmentsApiController(ApplicationDbContext context)
		{
			_context = context;
		}

		// ✅ GET: api/appointments
		[HttpGet]
		public IActionResult GetAllAppointments()
		{
			var appointments = _context.Appointments
				.Include(a => a.User)
				.Include(a => a.Trainer)
				.Include(a => a.Service)
				.OrderBy(a => a.StartTime)
				.Select(a => new
				{
					a.Id,
					MemberName = a.User.UserName,
					TrainerName = a.Trainer.Name,
					ServiceName = a.Service.Name,
					StartTime = a.StartTime,
					EndTime = a.EndTime,
					a.Status
				})
				.ToList();

			return Ok(appointments);
		}
	}
}
