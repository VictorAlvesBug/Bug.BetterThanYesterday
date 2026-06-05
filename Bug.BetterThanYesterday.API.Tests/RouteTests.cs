using Bug.BetterThanYesterday.API.Tests.Commons;
using Bug.BetterThanYesterday.API.Tests.RouteTestCases;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace Bug.BetterThanYesterday.API.Tests
{
	public class RouteTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
	{
		public static IEnumerable<object[]> RouteTestCases()
		{
			#region SetUpTestCases
			TestCases.SetUpNotFoundTestCases();
			TestCases.SetUpHappyPathTestCases();
			#endregion

			TestCases.CheckForDuplicatedRoutes();

			foreach (var testCase in TestCases.Routes)
				yield return new object[] { testCase };
		}

		[Theory]
		[MemberData(nameof(RouteTestCases))]
		public async Task Route_Should_Return_Expected_Result(Route testCase)
		{
			var testCaseNameToDebug = "ListPlansByFilter_WhenUserDoesNotExist_ShouldReturnNotFound";

			if (testCase.Name == testCaseNameToDebug)
			{
				Console.WriteLine("Debugging...");
			}

			// Arrange
			await fixture.ResetMockDataAsync(testCase.MocksSetUp);

			var request = new HttpRequestMessage
				{
					Method = testCase.Method,
					RequestUri = new Uri($"http://localhost:5018/testapi/{testCase.Path}"),
					Headers = {
						{ "accept", "*/*" },
					}
				};

				if (testCase.Method != HttpMethod.Get && testCase.Body is not null)
				{
					var jsonBody = JsonConvert.SerializeObject(testCase.Body);
					request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
				}

				// Act
				using var response = await fixture.Client.SendAsync(request);

				// Assert
				Assert.Equal(testCase.ExpectedStatusCode, (int)response.StatusCode);

				if (!string.IsNullOrEmpty(testCase.ExpectedMessageContains))
				{
					var responseBody = await response.Content.ReadAsStringAsync();
					Assert.Contains(testCase.ExpectedMessageContains, responseBody);
				}
		}
	}
}