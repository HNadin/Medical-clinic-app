using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Medical_clinic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        // Конструктор контролера AppRolesController.
        // Використовує Dependency Injection для отримання RoleManager.
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: AppRoles/Index
        // Метод для відображення списку всіх створених ролей користувачем.
        public IActionResult Index()
        {
            var roles = _roleManager.Roles; // Отримання всіх ролей з RoleManager.
            return View(roles);
        }

        // GET: AppRoles/Create
        // Метод для відображення сторінки створення нової ролі.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppRoles/Create
        // Метод для обробки створення нової ролі.
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            // Перевірка, чи роль із заданою назвою вже існує.
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                // Створення нової ролі із заданою назвою.
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }

            return RedirectToAction("Index"); // Перенаправлення на сторінку зі списком ролей після створення нової ролі.
        }
    }
}