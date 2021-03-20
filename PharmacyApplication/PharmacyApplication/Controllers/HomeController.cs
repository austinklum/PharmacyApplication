using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PharmacyApplication.Data;
using PharmacyApplication.Models;

namespace PharmacyApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContext _userContext;

        Random random;
        private List<String> SecurityQuestions = new List<string>{ "What is your mother's maiden name?",
                                                                   "Where did you go to highschool?",
                                                                   "What city were you born in?",
                                                                   "What is the make and model of your first car?",
                                                                   "Where was your first job?",
                                                                   "What was the name of your first pet?",
                                                                   "What was your childhood nickname?",
                                                                   "What was the first concert you attended?",
                                                                   "What street did you live on in third grade?",
                                                                   "What was your childhood best friend's name?" };

        private const string SecurityQuestionNum = "SecurityQuestionNum";
        private const string SecurityQuestionText = "SecurityQuestionText";
        private const string SecurityQuestionsAttempted = "SecurityQuestionsAttempted";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
