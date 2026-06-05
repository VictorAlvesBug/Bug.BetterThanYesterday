using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.API.Tests.Commons
{
	public static class MocksFactory
	{
		public static Habit CreateHabitWith(
			Guid id = default,
			string name = "",
			DateTime createdAt = default
		)
		{
			id = id == default ? Guid.NewGuid() : id;
			name = string.IsNullOrWhiteSpace(name) ? $"Habit {id.ToString()[..4]}" : name;
			createdAt = createdAt == default ? DateTime.Now : createdAt;

			return Habit.Restore(
				id,
				name,
				createdAt);
		}
		
		public static User CreateUserWith(
			Guid id = default,
			string name = "",
			string email = "",
			string? photoUrl = "",
			string nickname = "",
			string phoneNumber = "",
			string pixKey = "",
			string pixKeyType = "",
			DateTime createdAt = default
		)
		{
			id = id == default ? Guid.NewGuid() : id;
			name = string.IsNullOrWhiteSpace(name) ? $"User {id.ToString()[..4]}" : name;
			email = string.IsNullOrWhiteSpace(email) ? $"user{id.ToString()[..4]}@example.com" : email;
			photoUrl = string.IsNullOrWhiteSpace(photoUrl) ? $"user{id.ToString()[..4]}-photo.jpg" : photoUrl;
			nickname = string.IsNullOrWhiteSpace(nickname) ? $"user{id.ToString()[..4]}" : nickname;
			phoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? $"11987654321" : phoneNumber;
			pixKey = string.IsNullOrWhiteSpace(pixKey) ? phoneNumber : pixKey;
			pixKeyType = string.IsNullOrWhiteSpace(pixKeyType) ? $"PhoneNumber" : pixKeyType;
			createdAt = createdAt == default ? DateTime.Now : createdAt;

			return User.Restore(
				id,
				name,
				email,
				photoUrl,
				nickname,
				phoneNumber,
				pixKey,
				pixKeyType,
				createdAt);
		}

		public static Plan CreatePlanWith(
			Guid id = default,
			Guid ownerId = default,
			Guid habitId = default,
			string? description = "",
			DateTime startsAt = default,
			DateTime endsAt = default,
			string type = "Public",
			int daysOffPerWeek = 2,
			decimal penaltyValue = 10m,
			bool isCancelled = false,
			DateTime createdAt = default
		)
		{
			id = id == default ? Guid.NewGuid() : id;
			ownerId = ownerId == default ? Guid.NewGuid() : ownerId;
			habitId = habitId == default ? Guid.NewGuid() : habitId;
			description = string.IsNullOrWhiteSpace(description) ? $"Plan {id.ToString()[..4]} description" : description;
			startsAt = startsAt == default ? DateTime.Today.AddDays(1) : startsAt;
			endsAt = endsAt == default ? DateTime.Today.AddDays(8) : endsAt;
			createdAt = createdAt == default ? DateTime.Now : createdAt;

			return Plan.Restore(
				id,
				ownerId,
				habitId,
				description,
				startsAt,
				endsAt,
				type,
				daysOffPerWeek,
				penaltyValue,
				isCancelled,
				createdAt);
		}

		public static PlanMember CreatePlanMemberWith(
			Guid id = default,
			Guid planId = default,
			Guid userId = default,
			DateTime joinedAt = default,
			string status = "",
			DateTime createdAt = default
		)
		{
			id = id == default ? Guid.NewGuid() : id;
			planId = planId == default ? Guid.NewGuid() : planId;
			userId = userId == default ? Guid.NewGuid() : userId;
			joinedAt = joinedAt == default ? DateTime.Today.AddDays(-1) : joinedAt;
			status = string.IsNullOrWhiteSpace(status) ? "Active" : status;
			createdAt = createdAt == default ? DateTime.Now : createdAt;

			return PlanMember.Restore(
				id,
				planId,
				userId,
				joinedAt,
				status,
				createdAt);
		}

		public static CheckIn CreateCheckInWith(
			Guid id = default,
			Guid planId = default,
			Guid userId = default,
			DateTime date = default,
			int index = 1,
			string title = "",
			string photoUrl = "",
			string status = "",
			List<Review> reviews = default,
			DateTime createdAt = default
		)
		{
			id = id == default ? Guid.NewGuid() : id;
			planId = planId == default ? Guid.NewGuid() : planId;
			userId = userId == default ? Guid.NewGuid() : userId;
			date = date == default ? DateTime.Today : date;
			title = string.IsNullOrWhiteSpace(title) ? $"CheckIn {id.ToString()[..4]}" : title;
			photoUrl = string.IsNullOrWhiteSpace(photoUrl) ? $"checkin-{id.ToString()[..4]}-photo.jpg" : photoUrl;
			status = string.IsNullOrWhiteSpace(status) ? "Pending" : status;
			reviews = reviews == default ? [] : reviews;
			createdAt = createdAt == default ? DateTime.Now : createdAt;

			return CheckIn.Restore(
				id,
				planId,
				userId,
				date,
				index,
				title,
				photoUrl,
				status,
				reviews,
				createdAt);
		}
	}
}