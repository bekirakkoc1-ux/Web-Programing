using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eticaretmvc.Data;
using Eticaretmvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Eticaretmvc.Controllers
{
    public class SiparisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SiparisController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Gecmis()
        {
            var kullaniciId = HttpContext.Session.GetInt32("kullaniciId");

            if (kullaniciId == null)
                return RedirectToAction("Login", "Kullanici");

            var siparisler = _context.Siparisler
                .Include(s => s.Detaylar)
                .ThenInclude(d => d.Urun)
                .Where(s => s.KullaniciId == kullaniciId)
                .OrderByDescending(s => s.Tarih)
                .ToList();

            return View(siparisler);
        }
    }
}
