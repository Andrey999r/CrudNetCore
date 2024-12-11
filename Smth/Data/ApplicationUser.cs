namespace Smth.Data;

using System.Collections.Generic;

public class ApplicationUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    public List<Survey> Surveys { get; set; } = new List<Survey>();
}
