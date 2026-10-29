namespace Bug.BetterThanYesterday.Application.Challenges.Models;

public class RankingDayModel
{
	public DateTime Date { get; set; }
	public string Position { get; set; } = string.Empty;
	public string Tone { get; set; } = "gray-e";
}
