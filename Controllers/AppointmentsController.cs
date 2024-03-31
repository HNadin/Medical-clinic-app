using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Medical_clinic.Models;
using Medical_clinic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Medical_clinic.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationContext _context;
        private readonly Random _random;
        private readonly Stopwatch _stopwatch;

        // Конструктор контролера AppointmentsController.
        // Використовує Dependency Injection для отримання UserManager та ApplicationContext.
        public AppointmentsController(UserManager<IdentityUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
            _random = new Random();
            _stopwatch = new Stopwatch();

        }

        // GET: Appointments/Index
        // Метод для відображення списку призначень користувача.
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                // Отримання призначень користувача з бази даних, включаючи пов'язану послугу.
                var userAppointments = await _context.Appointments
                    .Where(a => a.UserId == currentUser.Id)
                    .Include(a => a.Service)
                    .ToListAsync(); // Використовуйте ToListAsync для асинхронного запиту до бази даних

                return View(userAppointments);
            }

            return View(new List<Appointment>());
        }

        // GET: Appointments/Statistics
        // Метод для відображення статистики призначень для адміністратора.
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics()
        {
            // Отримання статистики за послугами за поточний місяць та рік.
            var serviceStatistics = await _context.Appointments
                .Where(a => a.DateTime.Year == 2023)
                .GroupBy(a => a.Service.Name)
                .Select(g => new { ServiceName = g.Key, Count = g.Count() })
                .ToListAsync(); // Використовуйте ToListAsync для асинхронного запиту до бази даних

            // Отримання статистики за лікарями за поточний місяць та рік.
            var doctorStatistics = await _context.Appointments
                .Where(a => a.DateTime.Year == 2023)
                .GroupBy(a => a.Doctor)
                .Select(g => new { DoctorName = g.Key, Count = g.Count() })
                .ToListAsync(); // Використовуйте ToListAsync для асинхронного запиту до бази даних

            // Передача статистики у ViewBag для відображення на сторінці.
            ViewBag.ServiceStatistics = serviceStatistics;
            ViewBag.DoctorStatistics = doctorStatistics;

            return View();
        }
    }
}
