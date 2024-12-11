namespace Smth.ViewModel;

public class SurveyDetailsViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ParticipantViewModel> Participants { get; set; } = new();
}

public class ParticipantViewModel
{
    public string ParticipantName { get; set; }
    public List<AnswerViewModel> Answers { get; set; } = new();
}

public class AnswerViewModel
{
    public string Text { get; set; }
    public string ResponseText { get; set; }
}
