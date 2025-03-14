using Smth.Data;

namespace Smth.Interfaces;
public interface IEmailService
{
    void SendSurveyInvitation(string recipientEmail, string surveyLink);
}
