using MongoDB.Bson;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence;

public static class ChallengeCollectionMigration
{
	public static async Task ApplyAsync(IMongoDatabase database)
	{
		var names = await ListNamesAsync(database);
		await RenameIfNeededAsync(database, names, "plans", "challenges");
		names = await ListNamesAsync(database);
		await RenameIfNeededAsync(database, names, "plan_members", "challenge_members");
		names = await ListNamesAsync(database);

		foreach (var collection in new[]
		{
			"challenges",
			"challenge_members",
			"checkins",
			"penalties",
			"checkin_reports",
			"ledger_entries",
			"notifications"
		})
		{
			if (!names.Contains(collection))
				continue;

			var documents = database.GetCollection<BsonDocument>(collection);
			await documents.UpdateManyAsync(
				Builders<BsonDocument>.Filter.Exists("planId"),
				Builders<BsonDocument>.Update.Rename("planId", "challengeId"));
		}

		if (names.Contains("notifications"))
		{
			var notifications = database.GetCollection<BsonDocument>("notifications");
			await notifications.UpdateManyAsync(
				Builders<BsonDocument>.Filter.Exists("payload.planId"),
				Builders<BsonDocument>.Update.Rename("payload.planId", "payload.challengeId"));
		}
	}

	private static async Task<List<string>> ListNamesAsync(IMongoDatabase database)
	{
		using var cursor = await database.ListCollectionNamesAsync();
		return await cursor.ToListAsync();
	}

	private static async Task RenameIfNeededAsync(IMongoDatabase database, List<string> names, string from, string to)
	{
		if (names.Contains(from) && !names.Contains(to))
			await database.RenameCollectionAsync(from, to);
	}
}
