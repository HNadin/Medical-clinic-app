using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Medical_clinic.Data;
using Medical_clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Medical_clinic.Controllers
{
    public class NursesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Конструктор контролера NursesController.
        // Використовує Dependency Injection для отримання ApplicationContext та IWebHostEnvironment.
        public NursesController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Nurses/Index
        // Метод для відображення списку медсестер.
        public async Task<IActionResult> Index()
        {
            // Перевірка наявності даних в DbSet<Nurse>
            return _context.Nurses != null ?
                          View(await _context.Nurses.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Nurses'  is null.");
        }

        // GET: Nurses/Details/5
        // Метод для відображення деталей конкретної медсестри.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nurses == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // GET: Nurses/Create
        // Метод для відображення сторінки створення нової медсестри.
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Створення списку спеціалізацій вручну
            var specializations = new List<string>
            {
                "General Nursing",
                "Pediatric Nursing",
                "Cardiac Nursing",
                "Emergency Nursing",
                "Orthopedic Nursing",
                "Intensive Care Nursing",
                "Geriatric Nursing",
                "Oncology Nursing",
                "Pediatric Nursing",
                "Post-Anesthesia Nursin"
            };

            // Додавання порожнього вибору
            specializations.Insert(0, "");

            // Передача списку спеціалізацій у ViewBag для відображення на сторінці
            ViewBag.SpecializationList = new SelectList(specializations);

            return View();
        }

        // POST: Nurses/Create
        // Метод для обробки створення нової медсестри.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FullName,Education,CertificationTraining,Specialization,WorkScheduleData,PhotoPath")] Nurse nurse)
        {
            if (ModelState.IsValid)
            {
                if (nurse.PhotoPath != null)
                {
                    // Генерація унікального імені файлу для фотографії на основі імені медсестри
                    string uniqueFileName = $"{nurse.FullName.Replace(" ", "_").ToLower()}.jpg";

                    // Збереження фотографії в кореневій папці
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/nurses", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await nurse.PhotoPath.CopyToAsync(stream);
                    }

                    _context.Add(nurse);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(nurse);
        }

        // GET: Nurses/Edit/5
        // Метод для відображення сторінки редагування інформації про медсестру.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nurses == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }

            // Створення списку спеціалізацій вручну
            var specializations = new List<string>
            {
                "General Nursing",
                "Pediatric Nursing",
                "Cardiac Nursing",
                "Emergency Nursing",
                "Orthopedic Nursing",
                "Intensive Care Nursing",
                "Geriatric Nursing",
                "Oncology Nursing",
                "Pediatric Nursing",
                "Post-Anesthesia Nursin"
            };

            // Додавання порожнього вибору
            specializations.Insert(0, "");

            // Передача списку спеціалізацій у ViewBag для відображення на сторінці
            ViewBag.SpecializationList = new SelectList(specializations);

            return View(nurse);
        }

        // POST: Nurses/Edit/5
        // Метод для обробки редагування інформації про медсестру.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Education,CertificationTraining,Specialization,WorkScheduleData,PhotoPath")] Nurse nurse)
        {
            if (id != nurse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingNurse = await _context.Nurses.FindAsync(id);

                    if (existingNurse == null)
                    {
                        return NotFound();
                    }

                    // Оновлення властивостей, крім фотографії
                    _context.Entry(existingNurse).CurrentValues.SetValues(nurse);

                    if (nurse.PhotoPath != null)
                    {
                        // Видалення існуючої фотографії
                        string existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/nurses", $"{nurse.FullName.Replace(" ", "_").ToLower()}.jpg");

                        if (System.IO.File.Exists(existingImagePath))
                        {
                            System.IO.File.Delete(existingImagePath);
                        }

                        string uniqueFileName = $"{nurse.FullName.Replace(" ", "_").ToLower()}.jpg";

                        // Збереження нової фотографії
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/nurses", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await nurse.PhotoPath.CopyToAsync(stream);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NurseExists(nurse.Id))
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
            return View(nurse);
        }

        // GET: Nurses/Delete/5
        // Метод для відображення сторінки видалення медсестри.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nurses == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // POST: Nurses/Delete/5
        // Метод для обробки видалення медсестри.
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nurses == null)
            {
                return Problem("Entity set 'ApplicationContext.Nurses'  is null.");
            }

            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse != null)
            {
                // Побудова шляху до файлу зображення
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "nurses", $"{nurse.FullName.Replace(" ", "_").ToLower()}.jpg");

                if (System.IO.File.Exists(imagePath))
                {
                    // Видалення фотографії, якщо вона існує
                    System.IO.File.Delete(imagePath);
                }

                _context.Nurses.Remove(nurse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Метод для перевірки наявності медсестри з заданим ідентифікатором.
        private bool NurseExists(int id)
        {
            return (_context.Nurses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
