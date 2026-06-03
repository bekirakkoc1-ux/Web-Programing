using Microsoft.AspNetCore.Mvc;
using Eticaretmvc.Data;
using Eticaretmvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Eticaretmvc.Controllers
{
    public class AdminKategoriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminKategoriController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool AdminMi()
        {
            return HttpContext.Session.GetString("isAdmin") == "true";
        }

        public IActionResult Index()
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            var kategoriler = _context.Kategoriler.ToList();
            return View(kategoriler);
        }

        public IActionResult Create()
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kategori kategori)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            if (ModelState.IsValid)
            {
                _context.Kategoriler.Add(kategori);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        public IActionResult Edit(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            var kategori = _context.Kategoriler.Find(id);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Kategori kategori)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            if (ModelState.IsValid)
            {
                _context.Kategoriler.Update(kategori);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        public IActionResult Delete(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            var kategori = _context.Kategoriler.Find(id);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Kullanici");

            var kategori = _context.Kategoriler.Find(id);
            if (kategori != null)
            {
                _context.Kategoriler.Remove(kategori);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
