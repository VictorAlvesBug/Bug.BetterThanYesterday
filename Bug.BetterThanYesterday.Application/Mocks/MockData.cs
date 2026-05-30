using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Mocks
{
	public class MockData
	{

		public static readonly Guid HabitId0 = Guid.Parse("02acffc2-ce9c-408a-840e-748ddb787904");
		public static readonly Guid HabitId1 = Guid.Parse("0160269d-1e78-4ca2-b100-ee42805b5c1e");
		public static readonly Guid HabitId2 = Guid.Parse("f523e101-d4b9-453e-8669-c9e8a6918544");
		public static readonly Guid HabitId3 = Guid.Parse("f8cfc6a0-7304-41bb-985e-a3ce9c955bde");
		public static readonly Guid HabitId4 = Guid.Parse("809e7984-9eba-460e-be7d-955e229f7dce");

		public static readonly Guid UserId0 = Guid.Parse("52e253c0-fa75-4ae5-bf6f-02f9f4b7b853");
		public static readonly Guid UserId1 = Guid.Parse("57b8652a-81ad-46af-b50b-e1de389250da");
		public static readonly Guid UserId2 = Guid.Parse("814fbb49-66e1-4d51-a69e-bf1eb6d8fc4a");
		public static readonly Guid UserId3 = Guid.Parse("cc16329d-cbfc-4ef3-95bb-1b031179005f");
		public static readonly Guid UserId4 = Guid.Parse("78edf69e-bd58-4117-899d-be9150252d25");
		public static readonly Guid UserId5 = Guid.Parse("b7ddfa2f-1ca9-4f41-a105-c7170d4b1cc8");
		public static readonly Guid UserId6 = Guid.Parse("7cbe7e0c-61d0-4934-8482-cf17d4b0854f");

		public static readonly Guid PlanId0 = Guid.Parse("3bb93e8e78354ea1ba95e1877d037273");
		public static readonly Guid PublicRunningPlanId1_WithUserId1Active = Guid.Parse("40c8f170-b8b8-4e41-ac37-816750808650");
		public static readonly Guid PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active = Guid.Parse("a7f73852-db21-4791-94b0-1bcb55b0b496");
		public static readonly Guid PublicCancelledPlanId3 = Guid.Parse("bea8b9e8-5588-460e-bd5d-ae1c042bc166");
		public static readonly Guid PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked = Guid.Parse("79754103-5278-4ed2-afc5-bad44e97c4f6");
		public static readonly Guid PrivateFinishedPlanId5_WithUserId5Active = Guid.Parse("453f7331-6170-4cdd-912f-9ffc83a1ea8d");
		public static readonly Guid PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active = Guid.Parse("fb4e4d61-d64f-4dba-814b-c5e157776c15");
		public static readonly Guid PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active = Guid.Parse("5f63f6bc-bd97-47e7-b3d1-cb4eb64d9b26");

		public static readonly Guid PlanMemberId0 = PlanMember.BuildId(PlanId0, UserId0);
		public static readonly Guid PlanMemberId1 = PlanMember.BuildId(PublicRunningPlanId1_WithUserId1Active, UserId1);
		public static readonly Guid PlanMemberId2 = PlanMember.BuildId(PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserId1);
		public static readonly Guid PlanMemberId3 = PlanMember.BuildId(PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserId2);
		public static readonly Guid PlanMemberId4 = PlanMember.BuildId(PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserId3);
		public static readonly Guid PlanMemberId5 = PlanMember.BuildId(PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked, UserId2);
		public static readonly Guid PlanMemberId6 = PlanMember.BuildId(PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked, UserId3);
		public static readonly Guid PlanMemberId7 = PlanMember.BuildId(PrivateFinishedPlanId5_WithUserId5Active, UserId5);
		public static readonly Guid PlanMemberId8 = PlanMember.BuildId(PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active, UserId5);
		public static readonly Guid PlanMemberId9 = PlanMember.BuildId(PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserId3);
		public static readonly Guid PlanMemberId10 = PlanMember.BuildId(PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserId4);
		public static readonly Guid PlanMemberId11 = PlanMember.BuildId(PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserId5);
		public static readonly Guid PlanMemberId12 = PlanMember.BuildId(PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active, UserId4);

		public static readonly Guid CheckInId0 = Guid.Parse("c3c23a92-9e71-454d-bf12-71d3d4f95b06");
		public static readonly Guid CheckInId1 = Guid.Parse("d7c9f9d3-2b77-4c2c-a8d1-9b6f2b3d1a11");
		public static readonly Guid CheckInId2 = Guid.Parse("a13b9c7f-5f9a-4a2e-8b2c-3d1f4e5a2b22");
		public static readonly Guid CheckInId3 = Guid.Parse("c2f3b7e8-6d8f-4b1a-9c3d-7f2a1b4c3d33");
		public static readonly Guid CheckInId4 = Guid.Parse("c0834a21-9158-4658-b833-2655a974cc17");


		public static readonly List<Habit> MockHabits = [
			Habit.Restore(
				HabitId1,
				"Workout",
				new DateTime(1999, 01, 10)
			),
			Habit.Restore(
				HabitId2,
				"Reading",
				new DateTime(1967, 06, 20)
			),
			Habit.Restore(
				HabitId3,
				"Studying",
				new DateTime(2005, 04, 02)
			),
			Habit.Restore(
				HabitId4,
				"Cooking",
				new DateTime(1991, 01, 16)
			)
		];

		public static readonly List<User> MockUsers = [
			User.Restore(
				UserId1,
				"Ana",
				"ana@ex.com",
				null,
				"Ana",
				"11987654321",
				"11987654321",
				"PhoneNumber",
				new DateTime(2023, 06, 20)
			),
			User.Restore(
				UserId2,
				"Bob",
				"bob@ex.com",
				null,
				"Ana",
				"11987654321",
				"11987654321",
				"PhoneNumber",
				new DateTime(2024, 01, 10)
			),
			User.Restore(
				UserId3,
				"Carl",
				"carl@ex.com",
				null,
				"Ana",
				"11987654321",
				"11987654321",
				"PhoneNumber",
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId4,
				"David",
				"david@ex.com",
				null,
				"Ana",
				"11987654321",
				"11987654321",
				"PhoneNumber",
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId5,
				"Ellie",
				"ellie@ex.com",
				null,
				"Ana",
				"11987654321",
				"11987654321",
				"PhoneNumber",
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId6,
				"Fred",
				"fred@ex.com",
				null,
				"Ana",
				"11987654321",
				"11987654321",
				"PhoneNumber",
				new DateTime(2020, 06, 20)
			)
		];

		public static readonly List<Plan> MockPlans = [
			Plan.Restore(
				PublicRunningPlanId1_WithUserId1Active,
				UserId1,
				HabitId1,
				"Workout 5 times a week",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				PlanType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2024, 06, 15)
			),
			Plan.Restore(
				PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId1,
				HabitId2,
				"Reading 15 pages everyday",
				new DateTime(2026, 01, 01),
				new DateTime(2026, 12, 31),
				PlanType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2025, 10, 05)
			),
			Plan.Restore(
				PublicCancelledPlanId3,
				UserId1,
				HabitId3,
				"Studying AWS every weekend",
				new DateTime(2025, 01, 01),
				new DateTime(2025, 12, 31),
				PlanType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2020, 10, 05)
			),
			Plan.Restore(
				PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked,
				UserId1,
				HabitId3,
				"Studying React every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Plan.Restore(
				PrivateFinishedPlanId5_WithUserId5Active,
				UserId1,
				HabitId3,
				"Studying English every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Plan.Restore(
				PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserId1,
				HabitId3,
				"Studying Math every weekend",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				PlanType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Plan.Restore(
				PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId1,
				HabitId3,
				"Studying Anatomy every weekend",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				PlanType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			)
		];

		public static readonly List<PlanMember> MockPlanMembers = [
			PlanMember.Restore(
				PlanMemberId1,
				PublicRunningPlanId1_WithUserId1Active,
				UserId1,
				DateTime.Today,
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId2,
				PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId1,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId3,
				PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId2,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId4,
				PrivateNotStartedPlanId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId3,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId5,
				PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked,
				UserId2,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId6,
				PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked,
				UserId3,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId7,
				PrivateFinishedPlanId5_WithUserId5Active,
				UserId5,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId8,
				PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserId5,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId9,
				PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId3,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId10,
				PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId4,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId11,
				PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId5,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Active.Name,
				DateTime.Today
			),
			PlanMember.Restore(
				PlanMemberId12,
				PublicCancelledPlanId6_WithUserId4BlockedAndUserId5Active,
				UserId4,
				new DateTime(2020, 01, 01),
				PlanMemberStatus.Blocked.Name,
				DateTime.Today
			),
		];

		public static readonly List<CheckIn> MockCheckIns = [
			CheckIn.Restore(
					CheckInId1,
					PublicRunningPlanId1_WithUserId1Active,
					UserId1,
					DateTime.Today.AddDays(-1),
					1,
					"Morning workout",
					"Did 30 minutes of cardio",
					CheckInStatus.Pending.Name,
					[],
					DateTime.Today
				),
				CheckIn.Restore(
					CheckInId2,
					PublicRunningPlanId1_WithUserId1Active,
					UserId2,
					new DateTime(2025, 01, 05),
					1,
					"Reading",
					"Read 15 pages",
					CheckInStatus.Pending.Name,
					[],
					DateTime.Today
				),
				CheckIn.Restore(
					CheckInId3,
					PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
					UserId3,
					new DateTime(2025, 10, 10),
					1,
					"Evening review",
					"Reviewed notes",
					CheckInStatus.Pending.Name,
					[],
					DateTime.Today
				),
				CheckIn.Restore(
					CheckInId4,
					PublicRunningPlanId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
					UserId4,
					new DateTime(2025, 10, 10),
					1,
					"Evening review",
					"Reviewed notes",
					CheckInStatus.Pending.Name,
					[],
					DateTime.Today
				)
		];
	}
}