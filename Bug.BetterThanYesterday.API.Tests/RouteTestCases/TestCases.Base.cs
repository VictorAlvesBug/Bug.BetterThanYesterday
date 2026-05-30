using Bug.BetterThanYesterday.API.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Extensions;
using System.Collections.Generic;

namespace Bug.BetterThanYesterday.API.Tests.RouteTestCases;

public partial class TestCases
{
	internal static List<Route> Routes = [];

	public static void CheckForDuplicatedRoutes()
	{

		var dict = new Dictionary<string, List<string>>();

		var hasDuplicates = false;

		Routes.ForEach(route =>
		{
			var key = $"({route.Method}) {route.Path} - Body: {route.Body?.ToJson()}";

			if (dict.ContainsKey(key))
			{
				dict[key].Add(route.Name);
				hasDuplicates = true;
				return;
			}

			dict[key] = [route.Name];
		});

		if (hasDuplicates)
		{
			var duplicatedRoutes = dict.Where(kvp => kvp.Value.Count > 1).Select(kvp => $"{kvp.Value.JoinThis()} ({kvp.Value.Count}x {kvp.Key})");
			var strDuplicatedRoutes = duplicatedRoutes.JoinThis("\n");
			throw new Exception($"{duplicatedRoutes.Count()} rotas duplicadas: \n{strDuplicatedRoutes}");
		}

	}
}
