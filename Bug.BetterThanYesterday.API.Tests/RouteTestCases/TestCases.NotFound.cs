using Bug.BetterThanYesterday.API.Tests.Commons;
using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;
using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans.ListPlansByFilter;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Extensions;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Bug.BetterThanYesterday.API.Tests.RouteTestCases;

public partial class TestCases
{
	/*public T FillIfDefaultOrNull<T>(T value, T fillValue)
	{
		return value == default ? fillValue : value;
	}*/

	public static void SetUpNotFoundTestCases()
	{
		Routes.AddRange([
			new()
			{
				Name = "AddCheckIn_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(MockData.BaseAddCheckInCommand)
					.With(command => command.UserId, MockData.UserId1)
					.With(command => command.PlanId, MockData.PlanId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound,
				ExpectedMessageContains = Messages.PlanNotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(MockData.BaseAddCheckInCommand)
					.With(command => command.PlanId, MockData.PlanId0_WithNonExistingHabitIdRelated)
					.With(command => command.UserId, MockData.UserId1)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(MockData.BaseAddCheckInCommand)
					.With(command => command.UserId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddCheckIn_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(MockData.BaseAddCheckInCommand)
					.With(command => command.PlanId, MockData.PublicRunningPlanId1_WithUserId1Active)
					.With(command => command.UserId, MockData.UserId2)
					.Build(),
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
				Name = "GetCheckInById_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId0_WithNonExistingPlanIdRelated}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetCheckInById_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId0_WithNonExistingHabitIdRelated}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			/*new()// TODO Consertar teste
			{
				Name = "GetCheckInById_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId0_WithNonExistingOwnerIdRelated}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			/*new()
			{
				Name = "GetCheckInDetails_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetCheckInDetails_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetCheckInDetails_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetCheckInDetails_WhenCheckInDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserWithCheckInsByPlanIdAndUserId_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserWithCheckInsByPlanIdAndUserId_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserWithCheckInsByPlanIdAndUserId_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanUserWithCheckInsByPlanIdAndUserId_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanWithCheckInsByPlanId_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanWithCheckInsByPlanId_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanWithCheckInsByPlanId_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			new()
			{
				Name = "ListCheckInsByFilter_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{new MockBuilder<ListCheckInsByFilterCommand>(MockData.BaseListCheckInsByFilterCommand)
					.With(command => command.PlanId, MockData.PlanId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListCheckInsByFilter_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{new MockBuilder<ListCheckInsByFilterCommand>(MockData.BaseListCheckInsByFilterCommand)
					.With(command => command.UserId, MockData.UserId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListCheckInsByFilter_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{new MockBuilder<ListCheckInsByFilterCommand>(MockData.BaseListCheckInsByFilterCommand)
					.With(command => command.PlanId, MockData.PublicRunningPlanId1_WithUserId1Active)
					.With(command => command.UserId, MockData.UserId2)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			/*new()
			{
				Name = "ReviewCheckIn_WhenCheckInDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns/{MockData.CheckInId4}/Reviews",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns/{MockData.CheckInId4}/Reviews",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.With(command => command.ReviewerId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenCheckInOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.With(command => command.CheckInId, MockData.CheckInId0_WithNonExistingOwnerIdForCheckInRelated)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.With(command => command.CheckInId, MockData.CheckInId0_WithNonExistingPlanIdRelated)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.With(command => command.CheckInId, MockData.CheckInId0_WithNonExistingHabitIdRelated)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ReviewCheckIn_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.With(command => command.CheckInId, MockData.CheckInId1)
					.With(command => command.ReviewerId, MockData.UserId2)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "DeleteHabit_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Habits?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			new()
			{
				Name = "GetHabitById_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Habits/{MockData.HabitId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetHabitById_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Habits/{MockData.HabitId0_WithNonExistingPlanIdRelated}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			/*new()
			{
				Name = "UpdateHabit_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Habits",
				Body = new MockBuilder<UpdateHabitCommand>(MockData.BaseUpdateHabitCommand)
					.With(command => command.HabitId, MockData.HabitId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			new()
			{
				Name = "AddUserToPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "AddUserToPlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}/Members/{MockData.UserId1}",
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
				Name = "AddUserToPlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInThePlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInThePlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInThePlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInThePlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "BlockUserInThePlan_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId2}/Block",
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
				Name = "GetPlanMemberDetails_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}/Members/{MockData.UserId1}",
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
				Name = "GetPlanMemberDetails_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanMemberDetails_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId2}",
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
				Name = "GetPlanWithMembersByPlanId_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}/Members",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanWithMembersByPlanId_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}/Members",
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
				Name = "GetUserWithPlansByUserId_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Users/{MockData.UserId0_WithPlanId0AndOwnerId0}/Plans",
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
				Name = "RemoveUserFromPlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}/Members/{MockData.UserId1}",
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
				Name = "RemoveUserFromPlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "RemoveUserFromPlan_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId2}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInThePlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInThePlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInThePlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId0}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInThePlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}/Members/{MockData.UserId1}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "UnblockUserInThePlan_WhenPlanMemberDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId2}/Block",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			/*new()
			{
				Name = "CancelPlan_WhenPlanDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "CancelPlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "CancelPlan_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			new()
			{
				Name = "CreatePlan_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new MockBuilder<CreatePlanCommand>(MockData.BaseCreatePlanCommand)
					.With(command => command.OwnerId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "CreatePlan_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new MockBuilder<CreatePlanCommand>(MockData.BaseCreatePlanCommand)
					.With(command => command.HabitId, MockData.HabitId0)
					.Build(),
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
				Name = "GetPlanById_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingHabitIdRelated}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "GetPlanById_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanId0_WithNonExistingOwnerIdRelated}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListPlansByFilter_WhenOwnerDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans?{new MockBuilder<ListPlansByFilterCommand>(MockData.BaseListPlansByFilterCommand)
					.With(command => command.OwnerId, MockData.UserId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListPlansByFilter_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans?{new MockBuilder<ListPlansByFilterCommand>(MockData.BaseListPlansByFilterCommand)
					.With(command => command.HabitId, MockData.HabitId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			/*new()// TODO Consertar teste
			{
				Name = "ListPlansByFilter_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			/*new()
			{
				Name = "ListPlansByHabitId_WhenHabitDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "ListPlansByHabitId_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			new()
			{
				Name = "DeleteUser_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Delete,
				Path = $"Users?{0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},*/
			new()
			{
				Name = "GetUserById_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Get,
				Path = $"Users/{MockData.UserId0}",
				ExpectedStatusCode = StatusCodes.Status404NotFound
			},
			/*new()
			{
				Name = "UpdateUser_WhenUserDoesNotExist_ShouldReturnNotFound",
				Method = HttpMethod.Post,
				Path = $"Users",
				Body = new MockBuilder<UpdateUserCommand>(MockData.BaseUpdateUserCommand)
					.With(command => command.UserId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status404NotFound
			}*/
		]);
	}
}
