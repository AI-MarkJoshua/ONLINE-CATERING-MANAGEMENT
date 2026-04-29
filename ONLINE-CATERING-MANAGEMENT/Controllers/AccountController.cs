using Microsoft.AspNetCore.Mvc;
using ONLINE_CATERING_MANAGEMENT.Data;
using ONLINE_CATERING_MANAGEMENT.Models;
using ONLINE_CATERING_MANAGEMENT.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ONLINE_CATERING_MANAGEMENT.Controllers
{
    public class AccountController : Controller
    {
        private PasswordHasher<UserAccount> _passwordHasher = new PasswordHasher<UserAccount>();
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

            // Create user object FIRST
            var user = new UserAccount
            {
                Email = model.Email,
                Role = "Customer",
                IsVerified = false
            };

            // Hash password and store it
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            // 1️⃣ Validate form inputs
            if (!ModelState.IsValid)
                return View(model);

            // 2️⃣ Check if email exists in database
            var user = _context.UserAccounts
                .FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            // 3️⃣ Verify password hash
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                model.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            // 4️⃣ Create COOKIE (claims = user identity)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("UserID", user.UserAccountID.ToString())
    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            // 5️⃣ Sign in → create cookie 🍪
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            // 6️⃣ Redirect based on role
            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            if (user.Role == "Staff")
                return RedirectToAction("Dashboard", "Staff");

            return RedirectToAction("Dashboard", "Customer");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}