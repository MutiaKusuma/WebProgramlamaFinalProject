using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace WebProgramlamaFinalProject.Services

{
	public class NoOpEmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Task.CompletedTask;
		}
	}
}
