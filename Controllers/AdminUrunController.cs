using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eticaretmvc.Data;
using Eticaretmvc.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Eticaretmvc.Controllers
{
    public class AdminUrunController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminUrunController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var urunler = _context.Urunler.Include(u => u.Kategori).ToList();
            return View(urunler);
        }

        public IActionResult Create()
        {
            ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "Id", "Ad");
            return View();
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Urun urun, IFormFile resimDosyasi)
{
    if (ModelState.IsValid)
    {

        if (resimDosyasi != null && resimDosyasi.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
            var dosyaYolu = Path.Combine(uploadsFolder, dosyaAdi);

            using (var stream = new FileStream(dosyaYolu, FileMode.Create))
            {
                resimDosyasi.CopyTo(stream);
            }

            urun.ResimYolu = "/uploads/" + dosyaAdi;
        }

        _context.Urunler.Add(urun);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "Id", "Ad", urun.KategoriId);
    return View(urun);
}

        public IActionResult Edit(int id)
        {
            var urun = _context.Urunler.FirstOrDefault(x => x.Id == id);
            if (urun == null)
                return NotFound();

            ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "Id", "Ad", urun.KategoriId);
            return View(urun);
        }

       [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(Urun urun, IFormFile resimDosyasi)
{
    if (ModelState.IsValid)
    {
        var mevcutUrun = _context.Urunler.FirstOrDefault(u => u.Id == urun.Id);
        if (mevcutUrun == null)
            return NotFound();

        
        mevcutUrun.Ad = urun.Ad;
        mevcutUrun.Fiyat = urun.Fiyat;
        mevcutUrun.Aciklama = urun.Aciklama;
        mevcutUrun.KategoriId = urun.KategoriId;

        
        if (resimDosyasi != null && resimDosyasi.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
            var dosyaYolu = Path.Combine(uploadsFolder, dosyaAdi);

            using (var stream = new FileStream(dosyaYolu, FileMode.Create))
            {
                resimDosyasi.CopyTo(stream);
            }

            mevcutUrun.ResimYolu = "/uploads/" + dosyaAdi;
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    ViewBag.Kategoriler = new SelectList(_context.Kategoriler, "Id", "Ad", urun.KategoriId);
    return View(urun);
}
[HttpPost]
public IActionResult Delete(int id)
{
    // 1. Silinmek istenen ürünü veri tabanından buluyoruz
    var urun = _context.Urunler.FirstOrDefault(u => u.Id == id);
    
    if (urun != null)
    {
        // 2. Eğer ürüne ait eski bir görsel varsa wwwroot klasöründen de silelim (Temizlik için iyi olur)
        if (!string.IsNullOrEmpty(urun.ResimYolu))
        {
            var eskiDosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", urun.ResimYolu.TrimStart('/'));
            if (System.IO.File.Exists(eskiDosyaYolu))
            {
                System.IO.File.Delete(eskiDosyaYolu);
            }
        }

        // 3. Ürünü veri tabanından kaldırıp kaydediyoruz
        _context.Urunler.Remove(urun);
        _context.SaveChanges();
    }

    // 4. Silme işleminden sonra paneli yenilemek için Index sayfasına geri yönlendiriyoruz
    return RedirectToAction(nameof(Index));
}

    }
}
