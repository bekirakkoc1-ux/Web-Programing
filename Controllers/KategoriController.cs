using Microsoft.AspNetCore.Mvc;
using Eticaretmvc.Data;
using Eticaretmvc.Models;
using System.Linq;

namespace Eticaretmvc.Controllers
{
    public class KategoriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategoriController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var kategoriler = _context.Kategoriler.ToList();
            return View(kategoriler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kategori kategori)
        {
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
            var kategori = _context.Kategoriler.Find(id);
            if (kategori == null) return NotFound();
            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Kategori kategori)
        {
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
            var kategori = _context.Kategoriler.Find(id);
            if (kategori == null) return NotFound();
            return View(kategori);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
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
