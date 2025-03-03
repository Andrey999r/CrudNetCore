namespace Smth.Data;

using System.Collections.Generic;

public class Participant
{
    public int Id { get; set; }
    public string ParticipantName { get; set; }
    public int SurveyId { get; set; }
    public Survey Survey { get; set; }

    public List<Answer> Answers { get; set; } = new List<Answer>();
}
