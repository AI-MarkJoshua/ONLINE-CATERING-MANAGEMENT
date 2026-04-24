using Microsoft.AspNetCore.Mvc;
using ONLINE_CATERING_MANAGEMENT.Data;
using ONLINE_CATERING_MANAGEMENT.Models;
using ONLINE_CATERING_MANAGEMENT.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace ONLINE_CATERING_MANAGEMENT.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if email already exists
            var existingUser = _context.UserAccounts
                .FirstOrDefault(x => x.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(model);
            }

            // Hash password
            string hashedPassword = HashPassword(model.Password);

            var user = new UserAccount
            {
                Email = model.Email,
                PasswordHash = hashedPassword,
                Role = "Customer",
                IsVerified = false
            };

            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}