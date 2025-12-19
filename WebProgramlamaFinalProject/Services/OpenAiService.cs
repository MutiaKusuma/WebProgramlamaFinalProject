using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebProgramlamaFinalProject.Services
{
	public class OpenAiService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public OpenAiService(IConfiguration config)
		{
			_apiKey = config["OpenAI:ApiKey"];
			_httpClient = new HttpClient();
		}

		public async Task<string> GetFitnessRecommendation(
			int height,
			int weight,
			double bmi,
			string category)
		{
			var prompt = $@"
User data:
Height: {height} cm
Weight: {weight} kg
BMI: {bmi:F1}
Category: {category}

TASK:
Explain the BMI result in simple words.
Recommend estimated daily calorie intake.
Give a simple daily diet plan.
Suggest beginner-friendly exercises.

IMPORTANT RULES:
- Use PLAIN TEXT ONLY
- DO NOT use markdown
- DO NOT use symbols like ####, ###, **, or -
- Use numbered sections like:
1. BMI Explanation
2. Daily Calories
3. Diet Plan
4. Exercise Recommendation

Language: English
This is NOT medical advice.
";


			var requestBody = new
			{
				model = "gpt-4o-mini",
				messages = new[]
				{
					new { role = "user", content = prompt }
				}
			};

			var request = new HttpRequestMessage(
				HttpMethod.Post,
				"https://api.openai.com/v1/chat/completions");

			request.Headers.Authorization =
				new AuthenticationHeaderValue("Bearer", _apiKey);

			request.Content = new StringContent(
				JsonSerializer.Serialize(requestBody),
				Encoding.UTF8,
				"application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			using var doc = JsonDocument.Parse(json);

			return doc.RootElement
				.GetProperty("choices")[0]
				.GetProperty("message")
				.GetProperty("content")
				.GetString();
		}
	}
}
