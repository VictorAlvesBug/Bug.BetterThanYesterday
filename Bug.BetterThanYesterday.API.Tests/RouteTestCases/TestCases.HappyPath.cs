using Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;
using Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;
using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Application.Plans.CreatePlan;
using Bug.BetterThanYesterday.Application.Plans.ListPlansByFilter;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Extensions;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Bug.BetterThanYesterday.API.Tests.RouteTestCases;

public partial class TestCases
{
	public static void SetUpHappyPathTestCases()
	{
		Routes.AddRange([
			/*new()// TODO Consertar teste
			{
				Name = "AddCheckIn_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"CheckIns",
				Body = new MockBuilder<AddCheckInCommand>(MockData.BaseAddCheckInCommand)
					.With(command => command.PlanId, MockData.PublicRunningPlanId1_WithUserId1Active)
					.With(command => command.UserId, MockData.UserId1)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status201Created,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetCheckInById_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"CheckIns/{MockData.CheckInId1}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()
			{
				Name = "GetCheckInDetails_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},
			new()
			{
				Name = "GetPlanUserDateWithCheckInsByPlanIdAndUserIdAndDate_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},
			new()
			{
				Name = "GetPlanUserWithCheckInsByPlanIdAndUserId_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},
			new()
			{
				Name = "GetPlanWithCheckInsByPlanId_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{0}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "ListCheckInsByFilter_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"CheckIns?{new MockBuilder<ListCheckInsByFilterCommand>(MockData.BaseListCheckInsByFilterCommand)
					//.With(command => command.PlanId, MockData.PlanId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()
			{
				Name = "ReviewCheckIn_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"CheckIns/{MockData.CheckInId4}/Reviews",
				Body = new MockBuilder<ReviewCheckInCommand>(MockData.BaseReviewCheckInCommand)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status201Created,
				NeedsToResetMocksAfter = true
			},
			new()
			{
				Name = "DeleteHabit_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Delete,
				Path = $"Habits?{0}",
				ExpectedStatusCode = StatusCodes.Status204NoContent,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetHabitById_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Habits/{MockData.HabitId1}",
				ExpectedStatusCode = StatusCodes.Status201Created,
				NeedsToResetMocksAfter = true
			},*/
			/*new()
			{
				Name = "UpdateHabit_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"Habits",
				Body = new MockBuilder<UpdateHabitCommand>(MockData.BaseUpdateHabitCommand)
					.With(command => command.HabitId, MockData.HabitId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "AddUserToPlan_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status201Created,
				NeedsToResetMocksAfter = true
			},
			new()
			{
				Name = "BlockUserInThePlan_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"Plans/{MockData.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked}/Members/{MockData.UserId2}/Block",
				ExpectedStatusCode = StatusCodes.Status201Created,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetPlanMemberDetails_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetPlanWithMembersByPlanId_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked}/Members",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetUserWithPlansByUserId_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Users/{MockData.UserId1}/Plans",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "RemoveUserFromPlan_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PublicRunningPlanId1_WithUserId1Active}/Members/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status204NoContent,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "UnblockUserInThePlan_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Delete,
				Path = $"Plans/{MockData.PrivateFinishedPlanId4_WithUserId2ActiveAndUserId3Blocked}/Members/{MockData.UserId3}/Block",
				ExpectedStatusCode = StatusCodes.Status204NoContent,
				NeedsToResetMocksAfter = true
			},*/
			/*new()
			{
				Name = "CancelPlan_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Delete,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status204NoContent,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "CreatePlan_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"Plans",
				Body = new MockBuilder<CreatePlanCommand>(MockData.BaseCreatePlanCommand)
					//.With(command => command.OwnerId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status201Created,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetPlanById_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Plans/{MockData.PlanMemberId1_PlanId1AndUserId1Relation}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "ListPlansByFilter_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Plans?{new MockBuilder<ListPlansByFilterCommand>(MockData.BaseListPlansByFilterCommand)
					//.With(command => command.OwnerId, MockData.UserId0)
					.Build()
					.ToQueryString()}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()
			{
				Name = "ListPlansByHabitId_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Plans?{0}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},
			new()
			{
				Name = "DeleteUser_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Delete,
				Path = $"Users?{0}",
				ExpectedStatusCode = StatusCodes.Status204NoContent,
				NeedsToResetMocksAfter = true
			},*/
			/*new()// TODO Consertar teste
			{
				Name = "GetUserById_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Get,
				Path = $"Users/{MockData.UserId1}",
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			},*/
			/*new()
			{
				Name = "UpdateUser_WhenRequestIsValid_ShouldReturnSuccess",
				Method = HttpMethod.Post,
				Path = $"Users",
				Body = new MockBuilder<UpdateUserCommand>(MockData.BaseUpdateUserCommand)
					.With(command => command.UserId, MockData.UserId0)
					.Build(),
				ExpectedStatusCode = StatusCodes.Status200OK,
				NeedsToResetMocksAfter = true
			}*/
		]);
	}
}
