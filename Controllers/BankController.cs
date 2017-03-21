using System.Collections.Generic;
using System.Linq;
using BankAccounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.Controllers {
    public class BankController : Controller {
        private BankContext _context;

        public BankController (BankContext context) {
            _context = context;
        }

        [HttpGet]
        [Route ("Main")]
        public IActionResult Main () {
            List<User> AllUsers = _context.Users.ToList ();
            // Other code
            return View();
        }

    }
}