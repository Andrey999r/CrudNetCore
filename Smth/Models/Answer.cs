namespace Smth.Data;

public class Answer
{
    public int Id { get; set; }
    public string ResponseText { get; set; }

    public int? QuestionId { get; set; }
    public Question Question { get; set; }

    public int? ParticipantId { get; set; }
    public Participant Participant { get; set; }
}
