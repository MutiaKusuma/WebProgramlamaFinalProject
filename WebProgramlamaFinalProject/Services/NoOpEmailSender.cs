using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace WebProgramlamaFinalProject.Services

{
	public class NoOpEmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			// Tidak melakukan apa-apa
			return Task.CompletedTask;
		}
	}
}
