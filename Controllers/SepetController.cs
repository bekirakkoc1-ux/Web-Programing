using Microsoft.AspNetCore.Mvc;
using Eticaretmvc.Data;
using Eticaretmvc.Services;
using Eticaretmvc.Models;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Eticaretmvc.Controllers
{
    public class SepetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SepetService _sepetService;

        public SepetController(ApplicationDbContext context, SepetService sepetService)
        {
            _context = context;
            _sepetService = sepetService;
        }

        public IActionResult Index()
        {
            var sepet = _sepetService.SepetiListele();
            return View(sepet);
        }

        public IActionResult Ekle(int id)
        {
            var urun = _context.Urunler.FirstOrDefault(u => u.Id == id);
            if (urun != null)
            {
                _sepetService.SepeteEkle(urun);
            }
            return RedirectToAction("Index", "Sepet");
        }

        public IActionResult Sil(int id)
        {
            _sepetService.SepettenSil(id);
            return RedirectToAction("Index");
        }

        public IActionResult SatinAl()
{
    var kullaniciId = HttpContext.Session.GetInt32("kullaniciId");

    if (kullaniciId == null)
        return RedirectToAction("Login", "Kullanici");

    var sepet = _sepetService.SepetiListele();

    if (!sepet.Any())
    {
        TempData["Hata"] = "Sepetiniz boş.";
        return RedirectToAction("Index");
    }

    var siparis = new Siparis
    {
        KullaniciId = kullaniciId.Value,
        Tarih = DateTime.Now,
        Detaylar = sepet.Select(item => new SiparisDetay
        {
            UrunId = item.UrunId,
            Adet = item.Adet,
            Fiyat = item.Urun.Fiyat
        }).ToList()
    };

    _context.Siparisler.Add(siparis);
    _context.SaveChanges();

    _sepetService.SepetiTemizle();

    ViewBag.Mesaj = "Siparişiniz başarıyla oluşturuldu!";
    return View();
}

    }
}
