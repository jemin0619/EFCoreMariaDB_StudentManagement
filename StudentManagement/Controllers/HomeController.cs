using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Persistence.Model;
using StudentManagement.Persistence.Repository;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepository _studentRepository;

        public HomeController(ILogger<HomeController> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }

        // �л� ��� ��ȸ
        public async Task<IActionResult> Index()
        {
            var students = await _studentRepository.GetAsync();
            return View(students);
        }

        // �л� �� ��ȸ
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        // �л� ��� ��
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // �л� ��� ó��
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentEntity student)
        {
            if (ModelState.IsValid)
            {
                await _studentRepository.CreateAsync(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // �л� ���� ��
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        // �л� ���� ó��
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEntity student)
        {
            if (id != student.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                await _studentRepository.UpdateAsync(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // �л� ���� ��
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        // �л� ���� ó��
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            await _studentRepository.DeleteAsync(student);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
