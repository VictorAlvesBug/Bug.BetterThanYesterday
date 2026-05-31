using Microsoft.AspNetCore.Http;
using Xunit;

namespace Bug.BetterThanYesterday.API.Tests
{
	public class DatabaseFixture : IDisposable
	{
		internal readonly HttpClient Client = new();

		public DatabaseFixture()
		{
			PersistMockDataAsync()
				.GetAwaiter()
				.GetResult();
		}

		public void Dispose()
		{
			DeleteMockDataAsync()
				.GetAwaiter()
				.GetResult();
		}

		internal async Task PersistMockDataAsync()
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"http://localhost:5018/api/AdminSettings/PersistMockData"),
				Headers = {
					{ "accept", "*/*" },
				}
			};

			using var response = await Client.SendAsync(request);

			Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
		}

		private async Task DeleteMockDataAsync()
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Delete,
				RequestUri = new Uri($"http://localhost:5018/api/AdminSettings/DeleteMockData"),
				Headers = {
					{ "accept", "*/*" },
				}
			};

			using var response = await Client.SendAsync(request);

			Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
		}
	}
}
