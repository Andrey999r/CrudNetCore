using Microsoft.EntityFrameworkCore;
using Smth.Data;
using Smth.ViewModel;

namespace Smth.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

public class SurveysController : Controller
{
    private readonly ApplicationDbContext _context;
    public SurveysController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        int? userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        return View();
    }

    [HttpPost]
    public IActionResult Create(string name, string description, string[] questions)
    {
        int? userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var survey = new Survey
        {
            Name = name,
            Description = description,
            ApplicationUserId = userId.Value
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
        int? userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var survey = _context.Surveys
            .Where(s => s.ApplicationUserId == userId.Value && s.Id == id)
            .Select(s => new SurveyDetailsViewModel {
                Name = s.Name,
                Description = s.Description,
                Participants = s.Participants.Select(p => new ParticipantViewModel {
                    ParticipantName = p.ParticipantName,
                    Answers = p.Answers.Select(a => new AnswerViewModel {
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
        int? userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var survey = _context.Surveys
            .Include(s => s.Participants)
            .ThenInclude(p => p.Answers)
            .FirstOrDefault(s => s.Id == id);

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
        int? userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Account");

        ViewBag.ShareLink = Url.Action("TakeSurvey", "Surveys", new { id }, Request.Scheme);
        return View();
    }

    [HttpGet]
    public IActionResult TakeSurvey(int id)
    {
        var survey = _context.Surveys
            .Where(s => s.Id == id)
            .Select(s => new {
                s.Id,
                s.Name,
                s.Description,
                Questions = s.Questions.Select(q => new { q.Id, q.Text })
            })
            .FirstOrDefault();

        return View(survey);
    }

    [HttpPost]
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
            _context.Answers.Add(new Answer {
                ParticipantId = participant.Id,
                QuestionId = surveyQuestions[i].Id,
                ResponseText = answers[i]
            });
        }

        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    private int? GetCurrentUserId()
    {
        if (HttpContext.Session.TryGetValue("UserId", out byte[] userIdBytes))
        {
            return BitConverter.ToInt32(userIdBytes, 0);
        }
        return null;
    }
}
