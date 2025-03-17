namespace Smth.Data;

using System.Collections.Generic;

public class Participant
{
    public int Id { get; set; }
    public string Email { get; set; } 

    public DateTime CompletedAt { get; set; } =DateTime.UtcNow.AddHours(3);// Дата прохождения

    public string ParticipantName { get; set; }
    public int SurveyId { get; set; }
    public Survey Survey { get; set; }

    public List<Answer> Answers { get; set; } = new List<Answer>();
}
