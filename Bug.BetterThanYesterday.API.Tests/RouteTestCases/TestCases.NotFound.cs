using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Bug.BetterThanYesterday.API.Tests.RouteTestCases;

public partial class TestCases
{
	public static void SetUpNotFoundTestCases()
	{
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
				Path = $"Plans",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListPlansByFilter_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddPlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new CreatePlanCommand(
					ownerId: MockData.UserId0,
					habitId: MockData.HabitId1,
					description: "Mock Description",
					startsAt: DateTime.Today.AddDays(1),
					endsAt: DateTime.Today.AddDays(8),
					type: PlanType.Private.Name,
					daysOffPerWeek: 2,
					penaltyValue: 10
				),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddPlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new CreatePlanCommand(
					ownerId: MockData.UserId1,
					habitId: MockData.HabitId0,
					description: "Mock Description",
					startsAt: DateTime.Today.AddDays(1),
					endsAt: DateTime.Today.AddDays(8),
					type: PlanType.Private.Name,
					daysOffPerWeek: 2,
					penaltyValue: 10
				),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanMemberDetails_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanMemberDetails_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanWithMembersByPlanId_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetUserWithPlansByUserId_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddUserToPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"PlanMembers",
				Body = new {  },
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddUserToPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"PlanMembers",
				Body = new {  },
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenUserIsNotRelated_ShouldReturnNotFoundOrBadRequest",
				Method = HttpMethod.Delete,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"PlanMembers/{MockData.PlanMemberId0}",
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
				Path = $"CheckIns",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new {  },
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new {  },
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenCheckInDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenReviewerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
		]);
	}
}
