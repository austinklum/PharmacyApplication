using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PharmacyApplication.Data;
using PharmacyApplication.Models;

namespace PharmacyApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContext _userContext;
        private PharmacistContext _pharmacistContext;

        Random random;
        private List<String> SecurityQuestions = new List<string>{ "What city were you born in?",
                                                                   "What is the first name of your favorite schoolteacher?",
                                                                   "What is the short name of the high school you attended?",
                                                                   "Which year did you graduate from high school?",
                                                                   "What is the first name of your favorite singer?",
                                                                   "What is your favorite color?",
                                                                   "What is the first name of your mother’s sister?",
                                                                   "What is the first name of your father’s brother?",
                                                                   "In which year, your immediate elder sibling was born?",
                                                                   "In which year, your immediate younger sibling was born?",
                                                                   "What are the last four digits of your current phone number?",
                                                                   "Which city would you like to visit as your dream vacation?",
                                                                   "Which country would you to visit as your dream vacation?",
                                                                   "What was your birth month and date?",
                                                                   "What is your closest friend’s nickname?",
                                                                   "What is the first name of your first roommate?",
                                                                   "What is the name of the college you attended first?",
                                                                   "What is the name of the course you liked the most in your first year of college?",
                                                                   "What is the name of the course you liked the most in your first year of high school?",
                                                                   "What was the make of your first car?",
                                                                   "In which year, you first flew in an airplane?"};

        public const string SecurityQuestionNum = "SecurityQuestionNum";
        public const string SecurityQuestionText = "SecurityQuestionText";
        public const string SecurityQuestionsAttempted = "SecurityQuestionsAttempted";
        public static string UserId = "UserId";
        public static string Username = "Username";
        public static string Name = "Name";
        public static string IncorrectPasswordString = "IncorrectPasswordString";
        public static string Role = "Role";
        public static string IncludeProcessed = "IncludeProcessed";
        public static string DrugCountValidation = "DrugCountValidation";
        public static string PrescriptionFillValidation = "PrescriptionFillValidation";

        public HomeController(ILogger<HomeController> logger, UserContext context, PharmacistContext pharmacistContext)
        {
            _logger = logger;
            _userContext = context;
            _pharmacistContext = pharmacistContext;
            random = new Random();
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString(Username, "");
            HttpContext.Session.SetString(SecurityQuestionNum, "0");
            HttpContext.Session.SetString(SecurityQuestionsAttempted, "");
            return RedirectToAction("Login");
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

        public IActionResult Login()
        {
            HttpContext.Session.SetString(SecurityQuestionNum, "0");
            HttpContext.Session.SetString(SecurityQuestionsAttempted, "");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginAsync(User enteredUser)
        {
            HttpContext.Session.SetString(IncorrectPasswordString, "");
            if (ModelState.IsValid)
            {
                if (enteredUser.Username == null)
                {
                    enteredUser.Username = HttpContext.Session.GetString(Username);
                }
                var foundUser = _userContext.Users.FirstOrDefault(a => a.Username.Equals(enteredUser.Username));
                if (foundUser == null)
                {
                    HttpContext.Session.SetString(Username, "");
                    HttpContext.Session.SetString(IncorrectPasswordString, "Username or password is incorrect");
                    HttpContext.Session.SetString(SecurityQuestionNum, "0");
                    return View();
                }
                if(foundUser.Salt == null)
                {
                    byte[] newSalt = new byte[32];
                    random.NextBytes(newSalt);
                    foundUser.Salt = newSalt;
                    _userContext.Update(foundUser);
                    _userContext.SaveChanges();
                }
                if (foundUser.AccountStatus != 1)
                {
                    HttpContext.Session.SetString(Username, "");
                    HttpContext.Session.SetString(SecurityQuestionNum, "4");
                    return View();
                }
                SHA512 hasher = new SHA512Managed();
                //No security question responses, so check if password is correct
                if (enteredUser.SecQ1Response == null && enteredUser.SecQ2Response == null && enteredUser.SecQ3Response == null)
                //if (enteredUser.SecQ1Response == null && enteredUser.SecQ2Response == null && enteredUser.SecQ3Response == null && !HttpContext.Session.GetString(SecurityQuestionsAttempted).Contains("1"))
                {

                    byte[] saltedPwd = Encoding.ASCII.GetBytes(enteredUser.Password + Encoding.ASCII.GetString(foundUser.Salt));
                    byte[] saltedHashedPwd = hasher.ComputeHash(saltedPwd);


                    //if (foundUser.PasswordHash == null || foundUser.PasswordHash.SequenceEqual(saltedHashedPwd))
                    if (foundUser.PasswordHash.SequenceEqual(saltedHashedPwd))
                    {
                        //send to first security question
                        int nextQuestionNum = random.Next(1, 4);
                        HttpContext.Session.SetString(SecurityQuestionNum, nextQuestionNum.ToString());
                        HttpContext.Session.SetString(SecurityQuestionsAttempted, nextQuestionNum.ToString());
                        HttpContext.Session.SetString(Username, foundUser.Username);

                        switch (nextQuestionNum)
                        {
                            case 1:
                                HttpContext.Session.SetString(SecurityQuestionText, SecurityQuestions.ElementAt(foundUser.SecQ1Index));
                                break;
                            case 2:
                                HttpContext.Session.SetString(SecurityQuestionText, SecurityQuestions.ElementAt(foundUser.SecQ2Index));
                                break;
                            case 3:
                                HttpContext.Session.SetString(SecurityQuestionText, SecurityQuestions.ElementAt(foundUser.SecQ3Index));
                                break;
                            default:
                                throw new IndexOutOfRangeException("Expecting a question index of 1-3 inclusive, got " + nextQuestionNum);
                        }

                        return View(enteredUser);
                    }

                    HttpContext.Session.SetString(IncorrectPasswordString, "Username or password is incorrect");
                    return View(enteredUser);
                }

                byte[] saltedQ1 = Encoding.ASCII.GetBytes(enteredUser.SecQ1Response + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedQ1 = hasher.ComputeHash(saltedQ1);
                byte[] saltedQ2 = Encoding.ASCII.GetBytes(enteredUser.SecQ2Response + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedQ2 = hasher.ComputeHash(saltedQ2);
                byte[] saltedQ3 = Encoding.ASCII.GetBytes(enteredUser.SecQ3Response + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedQ3 = hasher.ComputeHash(saltedQ3);

                //if(foundUser.SecQ1ResponseHash == null || foundUser.SecQ2ResponseHash == null || foundUser.SecQ3ResponseHash == null)
                if ((enteredUser.SecQ1Response != null && saltedHashedQ1.SequenceEqual(foundUser.SecQ1ResponseHash)) ||
                   (enteredUser.SecQ2Response != null && saltedHashedQ2.SequenceEqual(foundUser.SecQ2ResponseHash)) ||
                   (enteredUser.SecQ3Response != null && saltedHashedQ3.SequenceEqual(foundUser.SecQ3ResponseHash)))
                {
                    HttpContext.Session.SetString(Role, "Pharmacist");
                    HttpContext.Session.SetString(UserId, foundUser.Id.ToString());
                    HttpContext.Session.SetString(Username, foundUser.Username);
                    Pharmacist pharmacist = _pharmacistContext.Pharmacists.First(p => p.UserId == foundUser.Id);
                    HttpContext.Session.SetString(Name, pharmacist.Name);
                    //send to user dashboard ;
                    return RedirectToAction("UserDashBoard");
                }

                //Check if all are wrong, lock account
                if (HttpContext.Session.GetString(SecurityQuestionsAttempted).Contains("1") &&
                   HttpContext.Session.GetString(SecurityQuestionsAttempted).Contains("2") &&
                   HttpContext.Session.GetString(SecurityQuestionsAttempted).Contains("3"))
                {
                    HttpContext.Session.SetString(SecurityQuestionNum, "4");
                    foundUser.AccountStatus = -1;

                    _userContext.Update(foundUser);
                    await _userContext.SaveChangesAsync();
                    return View();
                }
                //select a q, check that q hasn't been attempted, send to that question
                // Not a fan of this, but don't know how to do it better
                while (true)
                {
                    int nextQuestionNum = random.Next(1, 4);
                    string attempts = HttpContext.Session.GetString(SecurityQuestionsAttempted);
                    switch (nextQuestionNum)
                    {
                        case 1:
                            if (!attempts.Contains("1"))
                            {
                                HttpContext.Session.SetString(SecurityQuestionNum, nextQuestionNum.ToString());
                                HttpContext.Session.SetString(SecurityQuestionText, SecurityQuestions.ElementAt(foundUser.SecQ1Index));
                                HttpContext.Session.SetString(SecurityQuestionsAttempted, attempts + "1");
                                return View(enteredUser);
                            }
                            break;
                        case 2:
                            if (!attempts.Contains("2"))
                            {
                                HttpContext.Session.SetString(SecurityQuestionNum, nextQuestionNum.ToString());
                                HttpContext.Session.SetString(SecurityQuestionText, SecurityQuestions.ElementAt(foundUser.SecQ2Index));
                                HttpContext.Session.SetString(SecurityQuestionsAttempted, attempts + "2");
                                return View(enteredUser);
                            }
                            break;
                        case 3:
                            if (!attempts.Contains("3"))
                            {
                                HttpContext.Session.SetString(SecurityQuestionNum, nextQuestionNum.ToString());
                                HttpContext.Session.SetString(SecurityQuestionText, SecurityQuestions.ElementAt(foundUser.SecQ3Index));
                                HttpContext.Session.SetString(SecurityQuestionsAttempted, attempts + "3");
                                return View(enteredUser);
                            }
                            break;
                        default:
                            throw new IndexOutOfRangeException("Expecting a question index of 1-3 inclusive, got " + nextQuestionNum);
                    }
                }

            }
            return View(enteredUser);
        }

        public ActionResult UserDashBoard()
        {
            if (HttpContext.Session.GetString(Username) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult MyDetails()
        {
            User foundUser = _userContext.Users.First(u => u.Id.ToString() == HttpContext.Session.GetString(UserId));
            Pharmacist foundPharmacist = _pharmacistContext.Pharmacists.First(p => p.UserId == foundUser.Id);
            UserDetailsViewModel vm = new UserDetailsViewModel
            {
                CurrentUser = foundUser,
                CurrentPharmacist = foundPharmacist
            };
            return View(vm);
        }

        public ActionResult EditMyDetails()
        {
            User foundUser = _userContext.Users.First(u => u.Id.ToString() == HttpContext.Session.GetString(UserId));
            Pharmacist foundPharmacist = _pharmacistContext.Pharmacists.First(p => p.UserId == foundUser.Id);
            UserDetailsViewModel vm = new UserDetailsViewModel
            {
                CurrentUser = foundUser,
                CurrentPharmacist = foundPharmacist
            };
            vm.Questions = GetSelectListItems(SecurityQuestions);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditMyDetails(UserDetailsViewModel vm)
        {
            if (!ModelState.IsValid || vm.CurrentUser.SecQ2Index == vm.CurrentUser.SecQ1Index || vm.CurrentUser.SecQ3Index == vm.CurrentUser.SecQ1Index || vm.CurrentUser.SecQ3Index == vm.CurrentUser.SecQ2Index)
            {
                vm.Questions = GetSelectListItems(SecurityQuestions);
                return View(vm);
            }
            User foundUser = _userContext.Users.First(u => u.Id.ToString() == HttpContext.Session.GetString(UserId));
            Pharmacist foundPharmacist = _pharmacistContext.Pharmacists.First(p => p.UserId == foundUser.Id);


            SHA512 hasher = new SHA512Managed();

            if (!string.IsNullOrEmpty(vm.CurrentUser.Password))
            {
                byte[] saltedPwd = Encoding.ASCII.GetBytes(vm.CurrentUser.Password + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedPwd = hasher.ComputeHash(saltedPwd);
                foundUser.PasswordHash = saltedHashedPwd;
            }

            foundUser.SecQ1Index = vm.CurrentUser.SecQ1Index;
            foundUser.SecQ2Index = vm.CurrentUser.SecQ2Index;
            foundUser.SecQ3Index = vm.CurrentUser.SecQ3Index;

            if(!string.IsNullOrEmpty(vm.CurrentUser.SecQ1Response))
            {
                byte[] saltedQ1 = Encoding.ASCII.GetBytes(vm.CurrentUser.SecQ1Response + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedQ1 = hasher.ComputeHash(saltedQ1);
                foundUser.SecQ1ResponseHash = saltedHashedQ1;
            }
            if (!string.IsNullOrEmpty(vm.CurrentUser.SecQ2Response))
            {
                byte[] saltedQ2 = Encoding.ASCII.GetBytes(vm.CurrentUser.SecQ2Response + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedQ2 = hasher.ComputeHash(saltedQ2);
                foundUser.SecQ2ResponseHash = saltedHashedQ2;
            }
            if (!string.IsNullOrEmpty(vm.CurrentUser.SecQ3Response))
            {
                byte[] saltedQ3 = Encoding.ASCII.GetBytes(vm.CurrentUser.SecQ3Response + Encoding.ASCII.GetString(foundUser.Salt));
                byte[] saltedHashedQ3 = hasher.ComputeHash(saltedQ3);
                foundUser.SecQ3ResponseHash = saltedHashedQ3;
            }

            if(!string.IsNullOrEmpty(vm.CurrentPharmacist.Name))
            {
                foundPharmacist.Name = vm.CurrentPharmacist.Name;
            }
            if (!string.IsNullOrEmpty(vm.CurrentPharmacist.Pronouns))
            {
                foundPharmacist.Pronouns = vm.CurrentPharmacist.Pronouns;
            }

            _userContext.Users.Update(foundUser);
            _userContext.SaveChanges();
            _pharmacistContext.Pharmacists.Update(foundPharmacist);
            _pharmacistContext.SaveChanges();

            HttpContext.Session.SetString(Name, foundPharmacist.Name);
            HttpContext.Session.SetString(Username, foundUser.Username);

            return RedirectToAction("MyDetails");
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.SetString(Username, "");
            HttpContext.Session.SetString(SecurityQuestionNum, "0");
            HttpContext.Session.SetString(SecurityQuestionsAttempted, "");
            return RedirectToAction("Login");
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            for(int i = 0; i < elements.Count(); i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = elements.ElementAt(i)
                }); ;
            }

            return selectList;
        }
    }
}
