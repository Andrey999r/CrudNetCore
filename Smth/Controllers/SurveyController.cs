using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smth.Data;
using Smth.Interfaces;
using Smth.Models;
using Smth.Services;
using Smth.ViewModel;

namespace Smth.Controllers
{
    [Authorize]

    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public SurveysController(ApplicationDbContext? context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;

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

            return RedirectToAction("Created", "Surveys");
        }
        public IActionResult Created()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdd))
            {
                return BadRequest("Некорректный ID пользователя");
            }

            // Получаем опросы, связанные с данным пользователем
            var surveys = _context.Surveys.Where(s => s.ApplicationUserId == userIdd).ToList();
            return View(surveys);
        }

        public IActionResult Completed()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int.TryParse(userId, out int parsedUserId);

            var userEmail = _context.Users
                .Where(u => u.Id == parsedUserId)
                .Select(u => u.Email)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Не удалось получить email пользователя.");
            }

            var completedSurveys = _context.Surveys
                .Include(s => s.Owner) // Загружаем владельца
                .Include(s => s.Participants) // Загружаем участников
                .ThenInclude(p => p.Answers) // Загружаем ответы
                .ThenInclude(a => a.Question) // Загружаем вопросы
                .Where(s => s.Participants.Any(p => p.Email == userEmail || p.Email == userEmail)) // Проверяем и email
                .ToList();

            ViewBag.UserEmail = userEmail;

            return View(completedSurveys);
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
                        Id = p.Id, // добавляем Id!
                        ParticipantName = p.ParticipantName,
                        Email = p.Email, // добавляем Email!
                        CompletedAt = p.CompletedAt,
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

            return RedirectToAction("Created", "Surveys");
        }

        public IActionResult Share(int id)
        {
            var survey = _context.Surveys.FirstOrDefault(s => s.Id == id);
            if (survey == null) return NotFound();

            ViewBag.SurveyId = id;
            ViewBag.ShareLink = Url.Action("TakeSurvey", "Surveys", new { id }, Request.Scheme);

            return View();
        }


        [HttpPost]
        public IActionResult SendSurveyInvitation(int surveyId, string recipientEmail)
        {
            try
            {
                var survey = _context.Surveys.FirstOrDefault(s => s.Id == surveyId);
                if (survey == null)
                {
                    ViewBag.Message = "Опрос не найден.";
                    ViewBag.IsSuccess = false;
                    return View("ShareSurveyResult");
                }

                var surveyLink = Url.Action("TakeSurvey", "Surveys", new { id = surveyId }, Request.Scheme);
                _emailService.SendSurveyInvitation(recipientEmail, surveyLink);

                ViewBag.Message = "Приглашение успешно отправлено!";
                ViewBag.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка при отправке: " + ex.Message;
                ViewBag.IsSuccess = false;
            }

            return View("ShareSurveyResult");
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
        public IActionResult TakeSurvey(int id, string participantName, string participantEmail, string[] answers)
        {
            var survey = _context.Surveys.FirstOrDefault(s => s.Id == id);
            if (survey == null)
            {
                return NotFound("Опрос не найден.");
            }

            if (answers == null || answers.Length == 0)
            {
                return BadRequest("Вы не ответили ни на один вопрос.");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int.TryParse(userId, out int currentUserId);

            var userEmail = _context.Users
                .Where(u => u.Id == currentUserId)
                .Select(u => u.Email)
                .FirstOrDefault();

            // Проверяем, есть ли уже участие от этого пользователя
            var existingParticipant = _context.Participants
                .FirstOrDefault(p => p.SurveyId == id && (p.Email == userEmail || p.Email == participantEmail));

            if (existingParticipant == null)
            {
                existingParticipant = new Participant
                {
                    ParticipantName = participantName ?? "Аноним",
                    Email = userEmail ?? participantEmail, // Если пользователь авторизован, используем его email
                    SurveyId = id,
                    CompletedAt = DateTime.UtcNow.AddHours(3)
                };

                _context.Participants.Add(existingParticipant);
                _context.SaveChanges();
            }

            var surveyQuestions = _context.Questions
                .Where(q => q.SurveyId == id)
                .ToList();

            for (int i = 0; i < surveyQuestions.Count && i < answers.Length; i++)
            {
                _context.Answers.Add(new Answer
                {
                    ParticipantId = existingParticipant.Id,
                    QuestionId = surveyQuestions[i].Id,
                    ResponseText = answers[i]
                });
            }

            _context.SaveChanges();

            return User.Identity.IsAuthenticated
                ? RedirectToAction("Completed", "Surveys")
                : RedirectToAction("ThankYou", "Surveys", new { participantName = existingParticipant.ParticipantName });
        }



        [HttpPost]
        public IActionResult UpdateEmail(ParticipantViewModel model)
        {
            var participant = _context.Participants.Find(model.Id);
            if (participant == null)
            {
                return NotFound();
            }

            participant.Email = model.Email;
            _context.SaveChanges();

            TempData["Message"] = "Email успешно обновлен!";
            return RedirectToAction("Info", new { participantId = model.Id });
        }

        [HttpGet]
        public IActionResult Info(int participantId)
        {
            var participant = _context.Participants
                .Include(p => p.Answers)
                .ThenInclude(a => a.Question) // Убедимся, что вопрос загружается
                .FirstOrDefault(p => p.Id == participantId);

            if (participant == null)
                return NotFound();

            var viewModel = new ParticipantViewModel
            {
                Id = participant.Id,
                ParticipantName = participant.ParticipantName,
                Email = participant.Email,

                Answers = (participant.Answers ?? new List<Answer>()).Select(a => new AnswerViewModel
                {
                    Text = a.Question?.Text ?? "Нет данных", // Проверяем, что вопрос не null
                    ResponseText = a.ResponseText
                }).ToList()
            };

            return View(viewModel);
        }




        [HttpPost]
        public IActionResult DeleteParticipant(int participantId)
        {
            var participant = _context.Participants
                .Include(p => p.Answers) // Загружаем связанные ответы
                .FirstOrDefault(p => p.Id == participantId);

            if (participant == null)
                return NotFound();

            // Удаляем сначала связанные ответы
            _context.Answers.RemoveRange(participant.Answers);

            // Затем удаляем самого участника
            _context.Participants.Remove(participant);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Участник успешно удалён.";
            return RedirectToAction("Details", new { id = participant.SurveyId });
        }

        public IActionResult InfoSurvey(int participantId)
        {
            var participant = _context.Participants
                .Include(p => p.Survey) // Загружаем сам опрос
                .Include(p => p.Answers)
                .ThenInclude(a => a.Question) // Загружаем вопросы
                .FirstOrDefault(p => p.Id == participantId);

            if (participant == null)
                return NotFound("Участник не найден.");

            var viewModel = new ParticipantViewModel
            {
                Id = participant.Id,
                ParticipantName = participant.ParticipantName,
                Email = participant.Email,
                SurveyName = participant.Survey?.Name ?? "Неизвестный опрос",
                Answers = participant.Answers.Select(a => new AnswerViewModel
                {
                    Text = a.Question?.Text ?? "Нет данных",
                    ResponseText = a.ResponseText
                }).ToList()
            };

            return View(viewModel);
        }
        [AllowAnonymous] // Разрешаем доступ без авторизации
        public IActionResult ThankYou(string participantName)
        {
            ViewBag.ParticipantName = participantName;
            return View();
        }




    }
}