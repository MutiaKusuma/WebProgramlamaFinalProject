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

		[HttpGet]
		public IActionResult GetAllAppointments([FromQuery] string? status)
		{
			var appointmentsQuery = _context.Appointments
				.Include(a => a.User)
				.Include(a => a.Trainer)
				.Include(a => a.Service)
				.AsQueryable();

	
			if (!string.IsNullOrEmpty(status))
			{
				appointmentsQuery = appointmentsQuery
					.Where(a => a.Status == status);
			}

			var appointments = appointmentsQuery
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
