using Microsoft.EntityFrameworkCore;
using Smth.Data;
using Smth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Smth.Controllers
{
    [Authorize]
  public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SurveysController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string description, string[] questions)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
                return RedirectToAction("Login", "Account");

            int parsedUserId = int.Parse(userId);

            var survey = new Survey
            {
                Name = name,
                Description = description,
                ApplicationUserId = parsedUserId
            };

            foreach (var q in questions.Where(x => !string.IsNullOrEmpty(x)))
            {
                survey.Questions.Add(new Question { Text = q });
            }

            _context.Surveys.Add(survey);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
                return RedirectToAction("Login", "Account");

            int parsedUserId = int.Parse(userId);

            var survey = _context.Surveys
                .Where(s => s.ApplicationUserId == parsedUserId && s.Id == id)
                .Select(s => new SurveyDetailsViewModel
                {
                    Name = s.Name,
                    Description = s.Description,
                    Participants = s.Participants.Select(p => new ParticipantViewModel
                    {
                        ParticipantName = p.ParticipantName,
                        Answers = p.Answers.Select(a => new AnswerViewModel
                        {
                            Text = a.Question.Text,
                            ResponseText = a.ResponseText
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefault();

            if (survey == null)
                return NotFound();

            return View(survey);
        }

        public IActionResult Delete(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
                return RedirectToAction("Login", "Account");

            int parsedUserId = int.Parse(userId);

            var survey = _context.Surveys
                .Include(s => s.Participants)
                .ThenInclude(p => p.Answers)
                .FirstOrDefault(s => s.Id == id && s.ApplicationUserId == parsedUserId);

            if (survey != null)
            {
                foreach (var participant in survey.Participants)
                {
                    _context.Answers.RemoveRange(participant.Answers);
                }
                _context.Participants.RemoveRange(survey.Participants);
                _context.Surveys.Remove(survey);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Share(int id)
        {
            var shareLink = Url.Action("TakeSurvey", "Surveys", new { id }, Request.Scheme);
            ViewBag.ShareLink = shareLink;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TakeSurvey(int id)
        {
            var survey = _context.Surveys
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Description,
                    Questions = s.Questions.Select(q => new { q.Id, q.Text })
                })
                .FirstOrDefault();

            return View(survey);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult TakeSurvey(int id, string participantName, string[] answers)
        {
            var survey = _context.Surveys.FirstOrDefault(s => s.Id == id);
            if (survey == null)
                return NotFound();

            var participant = new Participant { ParticipantName = participantName, SurveyId = id };
            _context.Participants.Add(participant);
            _context.SaveChanges();

            var surveyQuestions = _context.Questions.Where(q => q.SurveyId == id).ToList();
            for (int i = 0; i < surveyQuestions.Count && i < answers.Length; i++)
            {
                _context.Answers.Add(new Answer
                {
                    ParticipantId = participant.Id,
                    QuestionId = surveyQuestions[i].Id,
                    ResponseText = answers[i]
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
