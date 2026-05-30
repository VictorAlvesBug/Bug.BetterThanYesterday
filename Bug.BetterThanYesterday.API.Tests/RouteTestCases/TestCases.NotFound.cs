using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;
using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.PlanMembers.GetPlanMemberDetails;
using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans.ListPlansByFilter;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Extensions;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Bug.BetterThanYesterday.API.Tests.RouteTestCases;

public partial class TestCases
{
	public static void SetUpNotFoundTestCases()
	{
		var baseCreatePlanCommand = new CreatePlanCommand(
			ownerId: MockData.UserId1,
			habitId: MockData.HabitId1,
			description: "Mock Description",
			startsAt: DateTime.Today.AddDays(1),
			endsAt: DateTime.Today.AddDays(8),
			type: PlanType.Private.Name,
			daysOffPerWeek: 2,
			penaltyValue: 10
		);

		var baseListPlansByFilterCommand = new ListPlansByFilterCommand
		{
			HabitId = MockData.HabitId1,
			OwnerId = MockData.UserId1,
			Status = PlanStatus.Running.Name,
			Type = PlanType.Private.Name,
		};

		var baseListCheckInsByFilterCommand = new ListCheckInsByFilterCommand
		{
			PlanId = MockData.PublicRunningPlanId1_WithUserId1Active,
			UserId = MockData.UserId1,
			Date = DateTime.Today,
			Status = CheckInStatus.Pending.Name,
		};

		var baseAddCheckInCommand = new AddCheckInCommand
		{
			PlanId = MockData.PublicRunningPlanId1_WithUserId1Active,
			UserId = MockData.UserId1,
			Date = DateTime.Today,
			Title = "Mock Title",
			PhotoUrl = "Mock Url"
		};

		var baseReviewCheckInCommand = new ReviewCheckInCommand
		{
			CheckInId = MockData.CheckInId4,
			ReviewerId = MockData.UserId5,
			Date = DateTime.Now,
			Status = CheckInStatus.Rejected.Name
		};

		Routes.AddRange([
			new()
			{
				Name = "GetHabitById_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Habits/{MockData.HabitId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetUserById_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Users/{MockData.UserId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanById_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListPlansByFilter_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans?{new MockBuilder<ListPlansByFilterCommand>(baseListPlansByFilterCommand)
					.With(command => command.OwnerId, MockData.UserId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListPlansByFilter_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans?{new MockBuilder<ListPlansByFilterCommand>(baseListPlansByFilterCommand)
					.With(command => command.HabitId, MockData.HabitId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddPlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new MockBuilder<CreatePlanCommand>(baseCreatePlanCommand)
					.With(command => command.OwnerId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddPlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new MockBuilder<CreatePlanCommand>(baseCreatePlanCommand)
					.With(command => command.HabitId, MockData.HabitId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanMemberDetails_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanMemberDetails_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanWithMembersByPlanId_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0}/Members",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetUserWithPlansByUserId_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Users/{MockData.UserId0}/Plans",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddUserToPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddUserToPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenUserIsNotRelated_ShouldReturnNotFoundOrBadRequest",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId2}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetCheckInById_WhenCheckInDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListCheckInsByFilter_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{new MockBuilder<ListCheckInsByFilterCommand>(baseListCheckInsByFilterCommand)
					.With(command => command.PlanId, MockData.PlanId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(baseAddCheckInCommand)
					.With(command => command.PlanId, MockData.PlanId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(baseAddCheckInCommand)
					.With(command => command.UserId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenCheckInDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns/{MockData.CheckInId4}/Reviews",
				Body = new MockBuilder<ReviewCheckInCommand>(baseReviewCheckInCommand)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenReviewerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId4}/Reviews",
				Body = new MockBuilder<ReviewCheckInCommand>(baseReviewCheckInCommand)
					.With(command => command.ReviewerId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
		]);
	}
}
