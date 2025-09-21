using Bug.BetterThanYesterday.Application.DependencyInjection;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Bug.BetterThanYesterday.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NSubstitute;

namespace Bug.BetterThanYesterday.Application.Tests;

public class Tests
{

	[Test]
	public void ShouldValidadeAllDepencencies()
	{
		// Arrange
		var services = new ServiceCollection();

		var cfg = new DatabaseConfig { ConnectionString = "mongodb://fake", DatabaseName = "di-tests" };
		services.AddSingleton(Options.Create(cfg));
		services.AddSingleton<IDatabaseConfig>(_ => cfg);

		// Mock do IMongoDatabase
		var db = Substitute.For<IMongoDatabase>();
		services.AddSingleton(db);

		services.AddInfrastructureServices();
		services.AddApplicationServices();

		var provider = services.BuildServiceProvider(validateScopes: true);
		var errorsSet = new HashSet<string>();

		// Act
		foreach (var service in services)
		{
			if (service.ServiceType.IsGenericType && service.ServiceType.ContainsGenericParameters)
				continue;

			try
			{
				using var scope = provider.CreateScope();

				if (service.ImplementationType != null)
				{
					ActivatorUtilities.CreateInstance(scope.ServiceProvider, service.ImplementationType);
				}
				else if (service.ImplementationFactory != null)
				{
					service.ImplementationFactory(scope.ServiceProvider);
				}
			}
			catch (Exception ex)
			{
				errorsSet.Add($"Service: {service.ServiceType.FullName}\nError: {ex.Message}");
			}
		}

		// Assert
		Assert.That(errorsSet, Is.Empty, string.Join("\n\n", errorsSet));
	}
}