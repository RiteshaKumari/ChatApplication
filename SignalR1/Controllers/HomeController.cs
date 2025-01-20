using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR1.Models;
using SignalR1.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace SignalR1.Controllers
{
    public class HomeController : Controller
    {  //Utility ul = new Utility();
        private readonly ILogger<HomeController> _logger;
        private readonly MyChatDatabseContext dbContext;
        private readonly IHubContext<ChatHub> chatHubContext;
        private readonly HubConnectionService _hubConnectionService;

        public HomeController(
             ILogger<HomeController> logger,
             MyChatDatabseContext dbContext,
             IHubContext<ChatHub> chatHubContext,
         HubConnectionService hubConnectionService)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.chatHubContext = chatHubContext;
            _hubConnectionService = hubConnectionService;
        }

        public async Task<object> GetUserDetails(string email, string password)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return null;
            }

            var student = await dbContext.Students
                .FirstOrDefaultAsync(s => s.UserId == user.UserId);

            if (student != null)
            {
                return student;
            }

            var teacher = await dbContext.Teachers
                .FirstOrDefaultAsync(t => t.UserId == user.UserId);

            if (teacher != null)
            {
                return teacher;
            }

            return null;
        }

        public IActionResult Index()
        {
            if (Request.Cookies.TryGetValue("Role", out string role))
            {
                if (Request.Cookies.TryGetValue("UserId", out string id))
                {
                    if (role == "Teacher")
                    {
                        return RedirectToAction("TeacherDashboard", new { id = id, Role = "Teacher" });
                    }
                    else if (role == "Student")
                    {
                        return RedirectToAction("StudentDashboard", new { id = id, Role = "Student" });
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] LoginRequest loginRequest)
        {
                _logger.LogInformation($"Login attempt for email: {loginRequest.Email}");

            string email = loginRequest.Email;
            string password = loginRequest.Password;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Email or password is empty");
                return Json(new { success = false, message = "Email or password cannot be empty" });
            }

            var userDetails = await GetUserDetails(email, password);

            if (userDetails == null)
            {
                _logger.LogWarning("Invalid username or password");
                return Json(new { success = false, message = "Invalid username or password." });
            }

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            if (userDetails is Student student)
            {
                SetCookie("Username", student.Name, cookieOptions);
                SetCookie("Role", "Student", cookieOptions);
                SetCookie("UserId", student.UserId.ToString(), cookieOptions);

                _logger.LogInformation($"Student login successful: {student.StudentId}");
                return Json(new { success = true, message = "Login successful.", 
                    redirectUrl = Url.Action("StudentDashboard", new { id = student.StudentId , Role = "Student"}),
                    userId = student.UserId
                });
            }

            if (userDetails is Teacher teacher)
            {
                SetCookie("Username", teacher.Name, cookieOptions);
                SetCookie("UserId", teacher.UserId.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
                SetCookie("Role", "Teacher", cookieOptions);
              //  SetCookie("UserId", teacher.UserId.ToString(), cookieOptions);

                _logger.LogInformation($"Teacher login successful: {teacher.TeacherId}");
                return Json(new { success = true, message = "Login successful.", redirectUrl = Url.Action("TeacherDashboard", new { id = teacher.TeacherId, Role = "Teacher" }), userId = teacher.UserId });
            }

            _logger.LogError("An unexpected error occurred during login");
            return Json(new { success = false, message = "An unexpected error occurred." });
        }

        private void SetCookie(string key, string value, CookieOptions options)
        {
            Response.Cookies.Append(key, value, options);
        }

        public async Task<ActionResult> Signout()
        {
            // Remove the user's connection
            var connectionId = HttpContext.Items["ConnectionId"] as string; 
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubConnectionService.RemoveConnection(connectionId);
            }

            // Clear cookies
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            SetCookie("Username", "", cookieOptions);
            SetCookie("Role", "", cookieOptions);
            SetCookie("UserId", "", cookieOptions);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult StudentDashboard()
        {
            //if (Request.Cookies.TryGetValue("Role", out string role))
            //{
            //    if (Request.Cookies.TryGetValue("UserId", out string id))
            //    {
            //        if (role == "Student")
            //        {
                        return View();
            //        }
                 
            //    }
            //}
            //return View("Index");
        
        }

        public IActionResult TeacherDashboard()
        {
            //if (Request.Cookies.TryGetValue("Role", out string role))
            //{
            //    if (Request.Cookies.TryGetValue("UserId", out string id))
            //    {
            //        if (role == "Student")
            //        {
                        //return RedirectToAction("StudentDashboard", new { id = id });
            //        }
            //    }
            //}
            return View();
        }

        public IActionResult ChatRoom()
        {
            return View();
        }

        public IActionResult ChatDemo()
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

