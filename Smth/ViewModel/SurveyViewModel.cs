namespace Smth.ViewModel;

public class SurveyDetailsViewModel
{
    public int Id { get; set; }  // ✅ Добавили идентификатор опроса

    public string Name { get; set; }
    public string Description { get; set; }
    public List<ParticipantViewModel> Participants { get; set; } = new();
}

public class ParticipantViewModel
{
    public int Id { get; set; }  // Уникальный идентификатор
    public string ParticipantName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;  // Email участника
    public List<AnswerViewModel> Answers { get; set; } = new();
}


public class AnswerViewModel
{
    public string Text { get; set; }
    public string ResponseText { get; set; }
}
