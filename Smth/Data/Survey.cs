namespace Smth.Data;

using System.Collections.Generic;

public class Survey
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int ApplicationUserId { get; set; }
    public ApplicationUser Owner { get; set; }

    public List<Question> Questions { get; set; } = new List<Question>();
    public List<Participant> Participants { get; set; } = new List<Participant>();
}
