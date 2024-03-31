using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Medical_clinic.Data;
using Medical_clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Medical_clinic.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Конструктор контролера DoctorsController.
        // Використовує Dependency Injection для отримання ApplicationContext та IWebHostEnvironment.
        public DoctorsController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Doctors/Index
        // Метод для відображення списку лікарів.
        public async Task<IActionResult> Index()
        {
            // Перевірка наявності даних в DbSet<Doctor>
            return _context.Doctors != null ?
                        View(await _context.Doctors.ToListAsync()) :
                        Problem("Entity set 'ApplicationContext.Doctors'  is null.");
        }

        // GET: Doctors/Details/5
        // Метод для відображення деталей конкретного лікаря.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        // Метод для відображення сторінки створення нового лікаря.
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Створення списку спеціалізацій вручну
            var specializations = new List<string>
            {
                "ENT Specialist",
                "Cardiologist",
                "Pediatrician",
                "Orthopedic Surgeon",
                "Dermatologist",
                "Ophthalmologist",
                "Gastroenterologist",
                "Neurologist",
                "Urologist",
                "Psychiatrist",
                "Rheumatologist",
                "Allergist",
                "Pediatrician"
            };

            // Додавання порожнього вибору
            specializations.Insert(0, "");

            // Передача списку спеціалізацій у ViewBag для відображення на сторінці
            ViewBag.SpecializationList = new SelectList(specializations);

            return View();
        }

        // POST: Doctors/Create
        // Метод для обробки створення нового лікаря.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FullName,Education,CertificationTraining,Specialization,WorkScheduleData,PhotoPath")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                if (doctor.PhotoPath != null)
                {
                    // Генерація унікального імені файлу для фотографії на основі імені лікаря
                    string uniqueFileName = $"{doctor.FullName.Replace(" ", "_").ToLower()}.jpg";

                    // Збереження фотографії в кореневій папці
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/doctors", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await doctor.PhotoPath.CopyToAsync(stream);
                    }

                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        // Метод для відображення сторінки редагування інформації про лікаря.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            // Створення списку спеціалізацій вручну
            var specializations = new List<string>
            {
                "ENT Specialist",
                "Cardiologist",
                "Pediatrician",
                "Orthopedic Surgeon",
                "Dermatologist",
                "Ophthalmologist",
                "Gastroenterologist",
                "Neurologist",
                "Urologist",
                "Psychiatrist",
                "Rheumatologist",
                "Allergist",
                "Pediatrician"
            };

            // Додавання порожнього вибору
            specializations.Insert(0, "");

            // Передача списку спеціалізацій у ViewBag для відображення на сторінці
            ViewBag.SpecializationList = new SelectList(specializations);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // Метод для обробки редагування інформації про лікаря.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Education,CertificationTraining,Specialization,WorkScheduleData,PhotoPath")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDoctor = await _context.Doctors.FindAsync(id);

                    if (existingDoctor == null)
                    {
                        return NotFound();
                    }

                    // Оновлення властивостей, крім фотографії
                    _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);

                    if (doctor.PhotoPath != null)
                    {
                        // Видалення існуючої фотографії
                        string existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/doctors", $"{doctor.FullName.Replace(" ", "_").ToLower()}.jpg");

                        if (System.IO.File.Exists(existingImagePath))
                        {
                            System.IO.File.Delete(existingImagePath);
                        }

                        string uniqueFileName = $"{doctor.FullName.Replace(" ", "_").ToLower()}.jpg";

                        // Збереження нової фотографії
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/doctors", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doctor.PhotoPath.CopyToAsync(stream);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        // Метод для відображення сторінки видалення лікаря.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        // Метод для обробки видалення лікаря.
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctors == null)
            {
                return Problem("Entity set 'ApplicationContext.Doctors' is null.");
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                // Побудова шляху до файлу зображення
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "doctors", $"{doctor.FullName.Replace(" ", "_").ToLower()}.jpg");

                if (System.IO.File.Exists(imagePath))
                {
                    // Видалення фотографії, якщо вона існує
                    System.IO.File.Delete(imagePath);
                }

                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод для перевірки наявності лікаря з заданим ідентифікатором.
        private bool DoctorExists(int id)
        {
            return (_context.Doctors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
