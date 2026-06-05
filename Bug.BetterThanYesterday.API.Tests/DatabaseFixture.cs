using Bug.BetterThanYesterday.API.Tests.Commons;
using Bug.BetterThanYesterday.Application.DependencyInjection;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Bug.BetterThanYesterday.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace Bug.BetterThanYesterday.API.Tests
{
	public class DatabaseFixture : IDisposable
	{
		internal readonly HttpClient Client = new();

		private readonly IHabitRepository _habitRepository;
		private readonly IUserRepository _userRepository;
		private readonly IPlanRepository _planRepository;
		private readonly IPlanMemberRepository _planMemberRepository;
		private readonly ICheckInRepository _checkInRepository;


		public DatabaseFixture()
		{
			var services = new ServiceCollection();

			var cfg = new DatabaseConfig
			{
				ConnectionString = "mongodb://localhost:27017",
				DatabaseName = /*"better-than-yesterday",
				TestDatabaseName = */"better-than-yesterday-test"
			};
			services.AddSingleton(Options.Create(cfg));
			services.AddSingleton<IDatabaseConfig>(_ => cfg);

			// Mock do IMongoDatabase
			var db = Substitute.For<IMongoDatabase>();
			services.AddSingleton(db);

			services.AddInfrastructureServices();
			services.AddApplicationServices();

			var provider = services.BuildServiceProvider();

			_habitRepository = provider.GetRequiredService<IHabitRepository>();
			_userRepository = provider.GetRequiredService<IUserRepository>();
			_planRepository = provider.GetRequiredService<IPlanRepository>();
			_planMemberRepository = provider.GetRequiredService<IPlanMemberRepository>();
			_checkInRepository = provider.GetRequiredService<ICheckInRepository>();
		}

		public void Dispose()
		{
			DeleteMockDataAsync()
				.GetAwaiter()
				.GetResult();
		}

		internal async Task ResetMockDataAsync(MocksCollection mocksCollection)
		{
			await DeleteMockDataAsync();
			await PersistMockDataAsync(mocksCollection);
		}

		private async Task PersistMockDataAsync(MocksCollection mocksCollection)
		{
			var tasks = new List<Task>
			{
				_habitRepository.InsertJsonAsync(mocksCollection.Habits),
				_userRepository.InsertJsonAsync(mocksCollection.Users),
				_planRepository.InsertJsonAsync(mocksCollection.Plans),
				_planMemberRepository.InsertJsonAsync(mocksCollection.PlanMembers),
				_checkInRepository.InsertJsonAsync(mocksCollection.CheckIns)
			};

			await Task.WhenAll(tasks);
		}

		private async Task DeleteMockDataAsync()
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Delete,
				RequestUri = new Uri($"http://localhost:5018/testapi/AdminSettings/DeleteMockData"),
				Headers = {
					{ "accept", "*/*" },
				}
			};

			using var response = await Client.SendAsync(request);

			Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
		}
	}
}
