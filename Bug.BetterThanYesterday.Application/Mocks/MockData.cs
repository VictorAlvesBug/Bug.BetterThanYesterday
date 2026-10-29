using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;
using Bug.BetterThanYesterday.Application.Challenges.CreateChallenge;
using Bug.BetterThanYesterday.Application.Challenges.ListChallengesByFilter;
using Bug.BetterThanYesterday.Application.Users.UpdateUser;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Mocks
{
	public class MockData
	{

		#region Mock Ids

		public static readonly Guid HabitId0 = Guid.Parse("02acffc2-ce9c-408a-840e-748ddb787904");
		public static readonly Guid HabitId0_WithNonExistingChallengeIdRelated = Guid.Parse("f3c7d661-30b5-4ceb-a5da-57e2db41f0b1");
		public static readonly Guid HabitId1 = Guid.Parse("0160269d-1e78-4ca2-b100-ee42805b5c1e");
		public static readonly Guid HabitId2 = Guid.Parse("f523e101-d4b9-453e-8669-c9e8a6918544");
		public static readonly Guid HabitId3 = Guid.Parse("f8cfc6a0-7304-41bb-985e-a3ce9c955bde");
		public static readonly Guid HabitId4 = Guid.Parse("809e7984-9eba-460e-be7d-955e229f7dce");

		public static readonly Guid UserId0 = Guid.Parse("52e253c0-fa75-4ae5-bf6f-02f9f4b7b853");
		public static readonly Guid UserId0_WithChallengeId0AndOwnerId0 = Guid.Parse("1a6d6c1a-ecce-49e9-a02c-e5ea68fb8a92");
		public static readonly Guid UserId1 = Guid.Parse("57b8652a-81ad-46af-b50b-e1de389250da");
		public static readonly Guid UserId2 = Guid.Parse("814fbb49-66e1-4d51-a69e-bf1eb6d8fc4a");
		public static readonly Guid UserId3 = Guid.Parse("cc16329d-cbfc-4ef3-95bb-1b031179005f");
		public static readonly Guid UserId4 = Guid.Parse("78edf69e-bd58-4117-899d-be9150252d25");
		public static readonly Guid UserId5 = Guid.Parse("b7ddfa2f-1ca9-4f41-a105-c7170d4b1cc8");
		public static readonly Guid UserId6 = Guid.Parse("7cbe7e0c-61d0-4934-8482-cf17d4b0854f");

		public static readonly Guid ChallengeId0 = Guid.Parse("3bb93e8e78354ea1ba95e1877d037273");
		public static readonly Guid ChallengeId0_WithNonExistingHabitIdRelated = Guid.Parse("599bcca6-768e-45e1-b3f1-a2a3cb5456b5");
		public static readonly Guid ChallengeId0_WithNonExistingOwnerIdRelated = Guid.Parse("c5fdaa82-dbfa-4c99-9671-96d25dac46ab");
		public static readonly Guid PublicRunningChallengeId1_WithUserId1Active = Guid.Parse("40c8f170-b8b8-4e41-ac37-816750808650");
		public static readonly Guid PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active = Guid.Parse("a7f73852-db21-4791-94b0-1bcb55b0b496");
		public static readonly Guid PublicCancelledChallengeId3 = Guid.Parse("bea8b9e8-5588-460e-bd5d-ae1c042bc166");
		public static readonly Guid PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked = Guid.Parse("79754103-5278-4ed2-afc5-bad44e97c4f6");
		public static readonly Guid PrivateFinishedChallengeId5_WithUserId5Active = Guid.Parse("453f7331-6170-4cdd-912f-9ffc83a1ea8d");
		public static readonly Guid PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active = Guid.Parse("fb4e4d61-d64f-4dba-814b-c5e157776c15");
		public static readonly Guid PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active = Guid.Parse("5f63f6bc-bd97-47e7-b3d1-cb4eb64d9b26");

		public static readonly Guid ChallengeMemberId0_ChallengeId0AndUserId0Relation = ChallengeMember.BuildId(ChallengeId0, UserId0);
		public static readonly Guid ChallengeMemberId0_ChallengeId0AndHabitId0AndUserId1Relation = ChallengeMember.BuildId(ChallengeId0_WithNonExistingHabitIdRelated, UserId1);
		public static readonly Guid ChallengeMemberId0_ChallengeId0AndOwnerId0AndUserId1Relation = ChallengeMember.BuildId(ChallengeId0_WithNonExistingOwnerIdRelated, UserId1);
		public static readonly Guid ChallengeMemberId1_ChallengeId1AndUserId1Relation = ChallengeMember.BuildId(PublicRunningChallengeId1_WithUserId1Active, UserId1);
		public static readonly Guid ChallengeMemberId2_ChallengeId2AndUserId1Relation = ChallengeMember.BuildId(PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserId1);
		public static readonly Guid ChallengeMemberId3_ChallengeId2AndUserId2Relation = ChallengeMember.BuildId(PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserId2);
		public static readonly Guid ChallengeMemberId4_ChallengeId2AndUserId3Relation = ChallengeMember.BuildId(PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active, UserId3);
		public static readonly Guid ChallengeMemberId5_ChallengeId4AndUserId2Relation = ChallengeMember.BuildId(PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked, UserId2);
		public static readonly Guid ChallengeMemberId6_ChallengeId4AndUserId3Relation = ChallengeMember.BuildId(PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked, UserId3);
		public static readonly Guid ChallengeMemberId7_ChallengeId5AndUserId5Relation = ChallengeMember.BuildId(PrivateFinishedChallengeId5_WithUserId5Active, UserId5);
		public static readonly Guid ChallengeMemberId8_ChallengeId6AndUserId5Relation = ChallengeMember.BuildId(PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active, UserId5);
		public static readonly Guid ChallengeMemberId9_ChallengeId7AndUserId3Relation = ChallengeMember.BuildId(PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserId3);
		public static readonly Guid ChallengeMemberId10_ChallengeId7AndUserId4Relation = ChallengeMember.BuildId(PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserId4);
		public static readonly Guid ChallengeMemberId11_ChallengeId7AndUserId5Relation = ChallengeMember.BuildId(PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active, UserId5);
		public static readonly Guid ChallengeMemberId12_ChallengeId6AndUserId4Relation = ChallengeMember.BuildId(PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active, UserId4);

		public static readonly Guid CheckInId0 = Guid.Parse("c3c23a92-9e71-454d-bf12-71d3d4f95b06");
		public static readonly Guid CheckInId0_WithNonExistingOwnerIdRelated = Guid.Parse("2924e1c0-27a1-4eaf-9ff0-cb0e1d85bf44");
		public static readonly Guid CheckInId0_WithNonExistingChallengeOwnerIdRelated = Guid.Parse("339f7fac-d0fc-4d3e-9ea7-60e019342a6d");
		public static readonly Guid CheckInId0_WithNonExistingChallengeIdRelated = Guid.Parse("bc9900e4-3722-43d6-ab4b-57d38ee3129a");
		public static readonly Guid CheckInId0_WithNonExistingHabitIdRelated = Guid.Parse("5e4d2f8f-2a4a-4ad6-98ba-dde6bc7c6e8f");
		public static readonly Guid CheckInId1 = Guid.Parse("d7c9f9d3-2b77-4c2c-a8d1-9b6f2b3d1a11");
		public static readonly Guid CheckInId2 = Guid.Parse("a13b9c7f-5f9a-4a2e-8b2c-3d1f4e5a2b22");
		public static readonly Guid CheckInId3 = Guid.Parse("c2f3b7e8-6d8f-4b1a-9c3d-7f2a1b4c3d33");
		public static readonly Guid CheckInId4 = Guid.Parse("c0834a21-9158-4658-b833-2655a974cc17");

		#endregion

		#region Mock Data

		public static readonly List<Habit> MockHabits =
		[
			Habit.Restore(
				HabitId0_WithNonExistingChallengeIdRelated,
				$"Habit {HabitId0_WithNonExistingChallengeIdRelated}",
				new DateTime(1999, 01, 10)
			),
			Habit.Restore(
				HabitId1,
				$"Habit {HabitId1}",
				new DateTime(1999, 01, 10)
			),
			Habit.Restore(
				HabitId2,
				$"Habit {HabitId2}",
				new DateTime(1967, 06, 20)
			),
			Habit.Restore(
				HabitId3,
				$"Habit {HabitId3}",
				new DateTime(2005, 04, 02)
			),
			Habit.Restore(
				HabitId4,
				$"Habit {HabitId4}",
				new DateTime(1991, 01, 16)
			)
		];

		public static readonly List<User> MockUsers =
		[
			User.Restore(
			UserId1,
				$"User {UserId1}",
				$"{UserId1.ToString()[..4]}@ex.com",
				null,
				$"User {UserId1} Nickname",
				"11987654321",
				"11987654321",
				nameof(PixKeyType.PhoneNumber),
				new DateTime(2023, 06, 20)
			),
			User.Restore(
				UserId2,
				$"User {UserId2}",
				$"{UserId2.ToString()[..4]}@ex.com",
				null,
				$"User {UserId2} Nickname",
				"11987654321",
				"11987654321",
				nameof(PixKeyType.PhoneNumber),
				new DateTime(2024, 01, 10)
			),
			User.Restore(
				UserId3,
				$"User {UserId3}",
				$"{UserId3.ToString()[..4]}@ex.com",
				null,
				$"User {UserId3} Nickname",
				"11987654321",
				"11987654321",
				nameof(PixKeyType.PhoneNumber),
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId4,
				$"User {UserId4}",
				$"{UserId4.ToString()[..4]}@ex.com",
				null,
				$"User {UserId4} Nickname",
				"11987654321",
				"11987654321",
				nameof(PixKeyType.PhoneNumber),
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId5,
				$"User {UserId5}",
				$"{UserId5.ToString()[..4]}@ex.com",
				null,
				$"User {UserId5} Nickname",
				"11987654321",
				"11987654321",
				nameof(PixKeyType.PhoneNumber),
				new DateTime(2020, 06, 20)
			),
			User.Restore(
				UserId6,
				$"User {UserId6}",
				$"{UserId6.ToString()[..4]}@ex.com",
				null,
				$"User {UserId6} Nickname",
				"11987654321",
				"11987654321",
				nameof(PixKeyType.PhoneNumber),
				new DateTime(2020, 06, 20)
			)
		];

		public static readonly List<Challenge> MockChallenges =
		[
			Challenge.Restore(
				PublicRunningChallengeId1_WithUserId1Active,
				UserId1,
				HabitId1,
				$"Challenge {PublicRunningChallengeId1_WithUserId1Active}",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				ChallengeType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2024, 06, 15)
			),
			Challenge.Restore(
				PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId1,
				HabitId2,
				$"Challenge {PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active}",
				new DateTime(2026, 01, 01),
				new DateTime(2026, 12, 31),
				ChallengeType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2025, 10, 05)
			),
			Challenge.Restore(
				PublicCancelledChallengeId3,
				UserId1,
				HabitId3,
				$"Challenge {PublicCancelledChallengeId3}",
				new DateTime(2025, 01, 01),
				new DateTime(2025, 12, 31),
				ChallengeType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2020, 10, 05)
			),
			Challenge.Restore(
				PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked,
				UserId1,
				HabitId3,
				$"Challenge {PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked}",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				ChallengeType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Challenge.Restore(
				PrivateFinishedChallengeId5_WithUserId5Active,
				UserId1,
				HabitId3,
				$"Challenge {PrivateFinishedChallengeId5_WithUserId5Active}",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				ChallengeType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Challenge.Restore(
				PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active,
				UserId1,
				HabitId3,
				$"Challenge {PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active}",
				new DateTime(2024, 01, 01),
				new DateTime(2024, 12, 31),
				ChallengeType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Challenge.Restore(
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId1,
				HabitId3,
				$"Challenge {PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active}",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				ChallengeType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Challenge.Restore(
				ChallengeId0_WithNonExistingHabitIdRelated,
				UserId1,
				HabitId0,
				$"Challenge {ChallengeId0_WithNonExistingHabitIdRelated}",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				ChallengeType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			),
			Challenge.Restore(
				ChallengeId0_WithNonExistingOwnerIdRelated,
				UserId0,
				HabitId0_WithNonExistingChallengeIdRelated,
				$"Challenge {ChallengeId0_WithNonExistingOwnerIdRelated}",
				new DateTime(2025, 01, 01),
				new DateTime(DateTime.Today.Year, 12, 31),
				ChallengeType.Public.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10,
				isCancelled: false,
				new DateTime(2021, 10, 05)
			)
		];

		public static readonly List<ChallengeMember> MockChallengeMembers = [
			ChallengeMember.Restore(
				ChallengeMemberId1_ChallengeId1AndUserId1Relation,
				PublicRunningChallengeId1_WithUserId1Active,
				UserId1,
				DateTime.Today,
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMember.BuildId(PublicRunningChallengeId1_WithUserId1Active, UserId5),
				PublicRunningChallengeId1_WithUserId1Active,
				UserId5,
				DateTime.Today,
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId2_ChallengeId2AndUserId1Relation,
				PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId1,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId3_ChallengeId2AndUserId2Relation,
				PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId2,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Blocked.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId4_ChallengeId2AndUserId3Relation,
				PrivateNotStartedChallengeId2_WithUserId1ActiveAndUserId2BlockedAndUser3Active,
				UserId3,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId5_ChallengeId4AndUserId2Relation,
				PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked,
				UserId2,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId6_ChallengeId4AndUserId3Relation,
				PrivateFinishedChallengeId4_WithUserId2ActiveAndUserId3Blocked,
				UserId3,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Blocked.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId7_ChallengeId5AndUserId5Relation,
				PrivateFinishedChallengeId5_WithUserId5Active,
				UserId5,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId8_ChallengeId6AndUserId5Relation,
				PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active,
				UserId5,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId9_ChallengeId7AndUserId3Relation,
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId3,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Blocked.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId10_ChallengeId7AndUserId4Relation,
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId4,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId11_ChallengeId7AndUserId5Relation,
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId5,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Active.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId12_ChallengeId6AndUserId4Relation,
				PublicCancelledChallengeId6_WithUserId4BlockedAndUserId5Active,
				UserId4,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Blocked.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId0_ChallengeId0AndHabitId0AndUserId1Relation,
				ChallengeId0_WithNonExistingHabitIdRelated,
				UserId1,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Blocked.Name,
				DateTime.Today
			),
			ChallengeMember.Restore(
				ChallengeMemberId0_ChallengeId0AndOwnerId0AndUserId1Relation,
				ChallengeId0_WithNonExistingOwnerIdRelated,
				UserId1,
				new DateTime(2020, 01, 01),
				ChallengeMemberStatus.Blocked.Name,
				DateTime.Today
			),
		];

		public static readonly List<CheckIn> MockCheckIns = [
			CheckIn.Restore(
				CheckInId1,
				PublicRunningChallengeId1_WithUserId1Active,
				UserId1,
				DateTime.Today.AddDays(-1),
				1,
				$"CheckIn {CheckInId1}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId2,
				PublicRunningChallengeId1_WithUserId1Active,
				UserId2,
				new DateTime(2025, 01, 05),
				1,
				$"CheckIn {CheckInId2}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId3,
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId3,
				new DateTime(2025, 10, 10),
				1,
				$"CheckIn {CheckInId3}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId4,
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId4,
				new DateTime(2025, 10, 10),
				1,
				$"CheckIn {CheckInId4}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId0_WithNonExistingOwnerIdRelated,
				PublicRunningChallengeId7_WithUserId3BlockedAndUserId4ActiveAndUserId5Active,
				UserId0,
				new DateTime(2025, 10, 10),
				1,
				$"CheckIn {CheckInId0_WithNonExistingOwnerIdRelated}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId0_WithNonExistingChallengeIdRelated,
				ChallengeId0,
				UserId1,
				new DateTime(2025, 10, 10),
				1,
				$"CheckIn {CheckInId0_WithNonExistingChallengeIdRelated}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId0_WithNonExistingHabitIdRelated,
				ChallengeId0_WithNonExistingHabitIdRelated,
				UserId1,
				new DateTime(2025, 10, 10),
				1,
				$"CheckIn {CheckInId0_WithNonExistingHabitIdRelated}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			),
			CheckIn.Restore(
				CheckInId0_WithNonExistingOwnerIdRelated,
				ChallengeId0_WithNonExistingOwnerIdRelated,
				UserId1,
				new DateTime(2025, 10, 10),
				1,
				$"CheckIn {CheckInId0_WithNonExistingOwnerIdRelated}",
				"photoUrl",
				CheckInStatus.Pending.Name,
				[],
				DateTime.Today
			)
		];

		#endregion

		#region Mock Commands

		public static readonly AddCheckInCommand BaseAddCheckInCommand = new AddCheckInCommand
		{
			ChallengeId = PublicRunningChallengeId1_WithUserId1Active,
			UserId = UserId1,
			Date = DateTime.Today,
			Title = "Mock Title",
			EvidencePhotoUrl = "Mock Url"
		};

		public static readonly ListCheckInsByFilterCommand BaseListCheckInsByFilterCommand = new ListCheckInsByFilterCommand
		{
			ChallengeId = PublicRunningChallengeId1_WithUserId1Active,
			UserId = UserId1,
			Date = DateTime.Today,
			Status = CheckInStatus.Pending.Name,
		};

		/*public static readonly ReviewCheckInCommand BaseReviewCheckInCommand = new ReviewCheckInCommand
		{
			CheckInId = CheckInId4,
			ReviewerId = UserId5,
			Date = DateTime.Now,
			Status = CheckInStatus.Rejected.Name
		};*/

		/*public static readonly UpdateHabitCommand BaseUpdateHabitCommand = new UpdateHabitCommand(
			habitId: HabitId1,
			name: "Mock Name"
		);*/

		public static readonly CreateChallengeCommand BaseCreateChallengeCommand = new CreateChallengeCommand(
				ownerId: UserId1,
				habitId: HabitId1,
				description: "Mock Description",
				startsAt: DateTime.Today.AddDays(1),
				endsAt: DateTime.Today.AddDays(8),
				type: ChallengeType.Private.Name,
				daysOffPerWeek: 2,
				penaltyValue: 10
			);

		public static readonly ListChallengesByFilterCommand BaseListChallengesByFilterCommand = new ListChallengesByFilterCommand
		{
			HabitId = HabitId1,
			OwnerId = UserId1,
			Status = ChallengeStatus.Running.Name,
			Type = ChallengeType.Private.Name,
		};

		public static readonly UpdateUserCommand BaseUpdateUserCommand = new UpdateUserCommand(
			userId: UserId1,
			name: "Mock Name",
			email: "mock@email.com",
			photoUrl: "https://example.com/photo.jpg",
			nickname: "MockNick",
			phoneNumber: "11987654321",
			pixKey: "11987654321",
			pixKeyType: nameof(PixKeyType.PhoneNumber)
		);

		#endregion

	}
}