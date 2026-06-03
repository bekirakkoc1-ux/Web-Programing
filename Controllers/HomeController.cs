using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eticaretmvc.Models;
using Eticaretmvc.Data;
using System.Linq;

namespace Eticaretmvc.Controllers;

public class HomeController : Controller
{
   private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var kategoriler = _context.Kategoriler.ToList();
        ViewBag.Kategoriler = kategoriler;

        return View();
    }

    public IActionResult Kategori(int id)
    {
        var kategoriler = _context.Kategoriler.ToList();
        ViewBag.Kategoriler = kategoriler;

        var urunler = _context.Urunler
            .Where(u => u.KategoriId == id)
            .ToList();

        ViewBag.KategoriAdi = _context.Kategoriler
            .FirstOrDefault(k => k.Id == id)?.Ad ?? "Kategori";

        return View("Kategori", urunler);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
