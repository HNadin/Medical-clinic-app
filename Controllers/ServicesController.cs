using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical_clinic.Data;
using Medical_clinic.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Medical_clinic.Observer;
using Microsoft.AspNetCore.Identity;

namespace Medical_clinic.Controllers
{
    public class ServicesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationContext _context;
        private readonly MedicalFacility _medicalFacility;

        public ServicesController(ApplicationContext context, MedicalFacility medicalFacility, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _medicalFacility = medicalFacility;
            _userManager = userManager;
        }

        // GET: Services/Index 
        public async Task<IActionResult> Index()
        {
            // Перевірка ролі користувача
            bool isAdmin = User.IsInRole("Admin");

            // Отримання даних про послуги
            List<Service> services = await _context.Services.ToListAsync();

            // Створення списку для збереження інформації про послуги та унікальні імена лікарів та медсестер
            List<ServiceViewModel> serviceViewModels = new List<ServiceViewModel>();

            foreach (var service in services)
            {
                var doctorsAndNurses = await _context.ProvideServices
                    .Where(ps => ps.ServiceId == service.Id)
                    .Include(ps => ps.Doctor)
                    .Include(ps => ps.Nurse)
                    .ToListAsync();

                // Використання Distinct() для вибору унікальних імен
                var uniqueDoctors = doctorsAndNurses.Select(ps => ps.Doctor.FullName).Distinct().ToList();
                var uniqueNurses = doctorsAndNurses.Select(ps => ps.Nurse.FullName).Distinct().ToList();

                if (uniqueDoctors.Count > 0 || uniqueNurses.Count > 0 || isAdmin)
                {
                    var serviceViewModel = new ServiceViewModel
                    {
                        Service = service,
                        Doctors = uniqueDoctors,
                        Nurses = uniqueNurses
                    };

                    serviceViewModels.Add(serviceViewModel);
                }
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                ViewBag.IsSubscribed = _context.Subscriptions.Any(s => s.UserId == userId && s.IsSubscribed);
            }
            else
            {
                ViewBag.IsSubscribed = false;
            }

            return View(serviceViewModels);
        }

        public IActionResult Create()
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName");
            ViewBag.Nurses = new SelectList(_context.Nurses, "Id", "FullName");

            return View();
        }

        // POST: Services/Create
        // Асинхронний метод для створення нової послуги та пов'язаних із нею асоціацій з лікарями та медсестрами.
        // Обмежений доступом тільки для користувачів з роллю "Admin".
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] Service service, int[] Doctors, int[] Nurses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service); // Додає нову послугу до контексту бази даних.
                await _context.SaveChangesAsync(); // Зберігає зміни в базі даних.

                if (Doctors != null || Nurses != null)
                {
                    foreach (var doctorId in Doctors)
                    {
                        foreach (var nurseId in Nurses)
                        {
                            var provideService = new ProvideService
                            {
                                ServiceId = service.Id, // Прив'язує асоціацію до відповідної послуги.
                                DoctorId = doctorId,
                                NurseId = nurseId
                            };
                            _context.Add(provideService); // Додає асоціацію лікаря та медсестри до контексту бази даних.
                        }
                    }
                }

                await _context.SaveChangesAsync(); // Зберігає зміни в базі даних.
                // Получаем список подписанных пользователей (только email)
                var subscribedUsers = _context.Subscriptions
                    .Where(s => s.IsSubscribed)
                    .Join(
                        _userManager.Users,
                        subscription => subscription.UserId,
                        user => user.Id,
                        (subscription, user) => user.Email
                    )
                    .ToList();

                // Вызываем метод Notify с новым списком подписанных пользователей
                _medicalFacility.Notify(new NewService { Name = service.Name, Description = service.Description }, subscribedUsers);


                return RedirectToAction(nameof(Index)); // Перенаправляє на сторінку зі списком послуг після створення нової.
            }

            // Якщо ModelState недійсний, підготовлює ViewBag для випадаючих списків лікарів і медсестер і повертає на сторінку створення.
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName");
            ViewBag.Nurses = new SelectList(_context.Nurses, "Id", "FullName");

            return View(service);
        }

        // GET: Services/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }


            // Отримати існуючі асоціації для послуги
            var existingAssociations = _context.ProvideServices
                .Where(ps => ps.ServiceId == service.Id)
                .Include(ps => ps.Doctor)
                .Include(ps => ps.Nurse)
                .ToList();

            // Створити екземпляр ServiceViewModel і заповнити його властивості
            var serviceViewModel = new ServiceViewModel
            {
                Service = service,
                Doctors = existingAssociations.Select(ps => ps.Doctor.FullName).Distinct().ToList(),
                Nurses = existingAssociations.Select(ps => ps.Nurse.FullName).Distinct().ToList()
            };

            // Заповнити списки випадаючих списків для лікарів і медсестерів
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName");
            ViewBag.Nurses = new SelectList(_context.Nurses, "Id", "FullName");

            return View(serviceViewModel);
        }

        // POST: Services/Edit/5
        // Щоб захистити від атак видалення, включте лише ті властивості, які ви хочете прив'язати.
        // Докладніше див. http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Service service, int[] Doctors, int[] Nurses)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingService = await _context.Services.FindAsync(id);

                    if (existingService == null)
                    {
                        return NotFound();
                    }

                    // Оновіть властивості
                    _context.Entry(existingService).CurrentValues.SetValues(service);

                    if (Doctors != null || Nurses != null)
                    {
                        var existingAssociations = _context.ProvideServices.Where(ps => ps.ServiceId == service.Id);

                        // Видаліть існуючі асоціації, якщо це необхідно
                        _context.ProvideServices.RemoveRange(existingAssociations);

                        // Додайте нові асоціації тільки, якщо вибрані врачі або медсестри
                        if (Doctors.Length > 0 || Nurses.Length > 0)
                        {
                            foreach (var doctorId in Doctors)
                            {
                                foreach (var nurseId in Nurses)
                                {
                                    var provideService = new ProvideService
                                    {
                                        ServiceId = service.Id,
                                        DoctorId = doctorId,
                                        NurseId = nurseId
                                    };
                                    _context.Add(provideService);
                                }
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Якщо ModelState недійсний, підготуйте ViewBag і поверніть користувача на сторінку редагування з повідомленнями про помилки
            ViewBag.Doctors = new SelectList(await _context.Doctors.ToListAsync(), "Id", "FullName");
            ViewBag.Nurses = new SelectList(await _context.Nurses.ToListAsync(), "Id", "FullName");

            return View(service);
        }

        // GET: Services/Delete/5
        // Асинхронний метод для видалення послуги з ідентифікатором id.
        // Обмежений доступом тільки для користувачів з роллю "Admin".
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound(); // Повертає 404, якщо ідентифікатор не вказано або послуга не знайдена в базі даних.
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound(); // Повертає 404, якщо послуга не знайдена в базі даних.
            }

            return View(service); // Показує сторінку підтвердження видалення послуги.
        }

        // POST: Services/Delete/5
        // Асинхронний метод для підтвердження видалення послуги з ідентифікатором id.
        // Обмежений доступом тільки для користувачів з роллю "Admin".
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'ApplicationContext.Services' is null."); // Повертає проблему, якщо набір даних про послуги вказує на null.
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service); // Видаляє послугу з контексту бази даних.
            }

            await _context.SaveChangesAsync(); // Зберігає зміни в базі даних.
            return RedirectToAction(nameof(Index)); // Перенаправляє на сторінку зі списком послуг після видалення.
        }

        // Приватний метод, який перевіряє наявність послуги з ідентифікатором id.
        // Використовується в методі DeleteConfirmed.
        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault(); // Повертає true, якщо послуга існує, інакше - false.
        }

        // GET: Services/GetDoctorsForService
        // Метод, який повертає унікальні імена лікарів для певної послуги.
        [HttpGet]
        public JsonResult GetDoctorsForService(int serviceId)
        {
            var doctors = _context.ProvideServices
                .Where(ps => ps.ServiceId == serviceId)
                .Select(ps => ps.Doctor.FullName)
                .Distinct()
                .ToList();

            return Json(doctors); // Повертає унікальні імена лікарів у форматі JSON.
        }

        // POST: Services/BookAppointment
        // Асинхронний метод для реєстрації нового запису на прийом
        [Authorize] // Обмеження доступу тільки для авторизованих користувачів
        [HttpPost]
        public async Task<IActionResult> BookAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Отримайте ідентифікатор користувача з клеймів

                // Створіть новий запис Appointment та заповніть його даними з моделі
                var appointment = new Appointment
                {
                    ServiceId = model.ServiceId,
                    UserId = userId,
                    Doctor = model.Doctor,
                    DateTime = model.DateTime,
                    Name = model.Name,
                    Phone = model.Phone
                };

                // Збережіть запис в базі даних
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Перенаправте користувача на сторінку з підтвердженням запису або виконайте інші відповідні дії
                return RedirectToAction("Confirmation", "Services");
            }

            // Якщо ModelState недійсний, поверніть користувача на форму з повідомленнями про помилки
            return View("Index");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}