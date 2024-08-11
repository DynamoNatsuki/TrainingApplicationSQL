using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using TrainingApplicationSQL.Models;

namespace TrainingApplicationSQL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database.Database _database;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _database = new Database.Database("Data Source=localhost;Initial Catalog=TrainingDiariesDB;Integrated Security=True;Encrypt=False");
        }

        public IActionResult Index()
        {
            List<Session> sessions = _database.GetAllSessions();
            return View(sessions);
        }

        public IActionResult Details(int id)
        {
            Session session = _database.GetSessionById(id);
            return View(session);
        }

        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Create(Session session)
        {
            _database.CreateSession(session.ExerciseName, session.Sets, session.Reps, session.Weight);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Session session = _database.GetSessionById(id);
            return View(session);
        }

        [HttpPost]
        public IActionResult Edit(int id, string exerciseName, int sets, int reps, int weight)
        {
            _database.EditSessionById(id, exerciseName, sets, reps, weight);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var session = _database.GetSessionById(id); 
            return View(session);
        }

        [HttpPost]
        public IActionResult DeleteSessionById(int id)
        {
            _database.DeleteSessionById(id);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
