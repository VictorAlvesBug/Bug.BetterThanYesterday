using Bug.BetterThanYesterday.API.Tests.Commons;
using Bug.BetterThanYesterday.API.Tests.RouteTestCases;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using Xunit;

namespace Bug.BetterThanYesterday.API.Tests
{
	public class RouteTests
	{
		private readonly HttpClient _client = new();

		public static IEnumerable<object[]> RouteTestCases()
		{
			TestCases.SetUpNotFoundTestCases();

			foreach (var testCase in TestCases.Routes)
				yield return new object[] { testCase };
		}

		[Theory]
		[MemberData(nameof(RouteTestCases))]
		public async Task Route_Should_Return_Expected_Result(Route testCase)
		{
			//await PersistMockDataAsync();

			Console.WriteLine($"Running: {testCase.Name}");
			Console.WriteLine($"({testCase.Method}) {testCase.Path}");

			var request = new HttpRequestMessage
			{
				Method = testCase.Method,
				RequestUri = new Uri($"http://localhost:5018/api/{testCase.Path}"),
				Headers = { { "accept", "*/*" }, }
			};
				
			if (testCase.Body is not null)
			{
				var jsonBody = JsonConvert.SerializeObject(testCase.Body);
				Console.WriteLine($"Body: {jsonBody}");
				request.Content = new StringContent(jsonBody);
			}

			using var response = await _client.SendAsync(request);

			Assert.Equal(testCase.ExpectedStatusCode, (int)response.StatusCode);

			if (!string.IsNullOrEmpty(testCase.ExpectedMessageContains))
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				Assert.Contains(testCase.ExpectedMessageContains, responseBody);
			}

			//await DeleteMockDataAsync();
		}

		//private async Task DeleteMockDataAsync()
		//{
		//	Console.WriteLine($"Deleting Mock Data...");

		//	var request1 = new HttpRequestMessage
		//	{
		//		Method = HttpMethod.Post,
		//		RequestUri = new Uri($"http://localhost:5018/api/AdminSettings/DeleteMockData"),
		//		Headers = { { "accept", "*/*" }, }
		//	};

		//	using var response1 = await _client.SendAsync(request1);
		//	response1.EnsureSuccessStatusCode();

		//}

		//private async Task PersistMockDataAsync()
		//{
		//	Console.WriteLine($"Persisting Mock Data...");

		//	var request2 = new HttpRequestMessage
		//	{
		//		Method = HttpMethod.Post,
		//		RequestUri = new Uri($"http://localhost:5018/api/AdminSettings/DeleteMockData"),
		//		Headers = { { "accept", "*/*" }, }
		//	};
			
		//	using var response2 = await _client.SendAsync(request2);
		//	response2.EnsureSuccessStatusCode();
		//}
	}
}