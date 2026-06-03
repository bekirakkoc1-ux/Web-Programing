using System.Linq;
using Eticaretmvc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UrunController : Controller
{
    private readonly ApplicationDbContext _context;

    public UrunController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var urunler = _context.Urunler.Include(x => x.Kategori).ToList();
        return View(urunler);
    }

    [HttpGet]
        public IActionResult Ara(string q)
        {
            var urunler = _context.Urunler
                .Include(x => x.Kategori)
                .Where(x => x.Ad.ToLower().Contains(q.ToLower()))
                .ToList();

            ViewBag.AramaKelimesi = q;
            return View(urunler);
        }
}
