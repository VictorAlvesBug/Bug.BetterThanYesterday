using Bug.BetterThanYesterday.Domain.Reports.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Reports;

internal sealed class CheckInReportMapper : IDocumentMapper<CheckInReport, CheckInReportDocument>
{
	public CheckInReportDocument ToDocument(CheckInReport report) => new()
	{
		Id = report.Id,
		CheckInId = report.CheckInId,
		PlanId = report.PlanId,
		ReporterId = report.ReporterId,
		TargetUserId = report.TargetUserId,
		Reason = report.Reason,
		CreatedAt = report.CreatedAt
	};

	public CheckInReport ToDomain(CheckInReportDocument document) =>
		CheckInReport.Restore(
			document.Id,
			document.CheckInId,
			document.PlanId,
			document.ReporterId,
			document.TargetUserId,
			document.Reason,
			document.CreatedAt);
}
