using System.Collections.Generic;
using System.Linq;
using BankAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BankAccounts.Controllers {
    public class UserController : Controller {
        private BankContext _context;

        public UserController (BankContext context) {
                _context = context;
            }
        [HttpGet]
        [Route ("Login")]
        public IActionResult Login () {
            List<string> Errors = new List<string> ();
            try {
                List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>> ("Errors");
                foreach (object error in Results) {
                    Errors.Add (error.ToString ());
                }
            } catch {

            }
            ViewBag.Errors = Errors;
            return View ("login");
        }
        [HttpPost]
        [Route ("Login")]
        public IActionResult Login (UserValidation user) {
            List<string> Errors = new List<string> ();
            if (user.Email == null) {
                Errors.Add ("E-Mail can not be blank.");
            }
            if (user.Password == null) {
                Errors.Add ("Password can not be blank.");
            }
            if (Errors.Count == 0) {
                User DBUser = user.ToUser();
                User Results = _context.Users.Where (u => u.Email == DBUser.Email).SingleOrDefault ();
                if (Results != null) {
                    if (Results.Password == DBUser.Password) {
                        HttpContext.Session.SetInt32 ("UserId", Results.UserId);
                        return RedirectToAction ("Main", "Bank");
                    }
                }
                Errors.Add ("Invalid Email / Password Combination.");
            }
            HttpContext.Session.SetObjectAsJson ("Errors", Errors);
            return RedirectToAction ("Login");
        }
        [HttpGet]
        [Route ("Register")]
        public IActionResult Register () {
                List<string> Errors = new List<string> ();
                try {
                    List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>> ("Errors");
                    foreach (object error in Results) {
                        Errors.Add (error.ToString ());
                    }
                } catch {

                }
                ViewBag.Errors = Errors;
                return View ("reg");
            }

        [HttpPost]
        [Route ("Register")]
        public IActionResult Register (UserValidation user) {
                List<string> Errors = new List<string> ();
                if (ModelState.IsValid) {
                    User Results = _context.Users.Where (u => u.Email == user.Email).SingleOrDefault ();
                    if (Results == null) {
                        _context.Add (user.ToUser());
                        _context.SaveChanges ();
                        Results = _context.Users.Where (u => u.Email == user.Email).SingleOrDefault ();
                        HttpContext.Session.SetInt32 ("UserId", Results.UserId);
                        return RedirectToAction ("Main", "Bank");
                    } else {
                        Errors.Add ("User already exists, try a different E-Mail");
                    }
                } else {
                    Dictionary<string, string> Error = new Dictionary<string, string> ();
                    foreach (string key in ViewData.ModelState.Keys) {
                        foreach (ModelError error in ViewData.ModelState[key].Errors) {
                            Errors.Add (error.ErrorMessage);
                        }
                    }
                }
                HttpContext.Session.SetObjectAsJson ("Errors", Errors);
                return RedirectToAction ("Register");
            }

        [HttpGet]
        [RouteAttribute("Logout")]
        public IActionResult Logout (){
            HttpContext.Session.Clear();
            return RedirectToAction("Home", "Index");
        }
    }
}