using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgramlamaFinalProject.Data;

namespace WebProgramlamaFinalProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TrainersApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public TrainersApiController(ApplicationDbContext context)
		{
			_context = context;
		}

		// ✅ GET: api/trainers
		[HttpGet]
		public IActionResult GetAllTrainers([FromQuery] string? service)
		{
			var trainersQuery = _context.Trainers
				.Include(t => t.TrainerServices)
					.ThenInclude(ts => ts.Service)
				.AsQueryable();

			if (!string.IsNullOrEmpty(service))
			{
				trainersQuery = trainersQuery
					.Where(t => t.TrainerServices.Any(ts => ts.Service.Name == service));
			}

			var trainers = trainersQuery
				.Select(t => new
				{
					t.Id,
					t.Name,
					Services = t.TrainerServices
						.Select(ts => ts.Service.Name)
						.ToList()
				})
				.ToList();

			return Ok(trainers);
		}

	}
}
