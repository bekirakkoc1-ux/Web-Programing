using Microsoft.AspNetCore.Mvc;
using Eticaretmvc.Data;
using Eticaretmvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Eticaretmvc.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KullaniciController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Kullanici kullanici)
        {
            var mevcut = _context.Kullanicilar.FirstOrDefault(x => x.Email == kullanici.Email);
            if (mevcut != null)
            {
                ModelState.AddModelError("", "Bu e-posta zaten kayıtlı.");
                return View();
            }

            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();

            HttpContext.Session.SetString("kullaniciAdi", kullanici.KullaniciAdi);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string sifre)
        {
            var kullanici = _context.Kullanicilar
                .FirstOrDefault(x => x.Email == email && x.Sifre == sifre);

            if (kullanici == null)
            {
                ModelState.AddModelError("", "Hatalı email veya şifre.");
                return View();
            }

            HttpContext.Session.SetString("kullaniciAdi", kullanici.KullaniciAdi);
            HttpContext.Session.SetString("isAdmin", kullanici.IsAdmin ? "true" : "false");
            HttpContext.Session.SetInt32("kullaniciId", kullanici.Id);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("kullaniciAdi");
            HttpContext.Session.Remove("isAdmin");
            HttpContext.Session.Remove("kullaniciId");
            return RedirectToAction("Login");
        }

        // 1. Sayfa açıldığında giriş yapmış kullanıcının bilgilerini getiren metot (GET)
        public IActionResult Ayarlar()
        {
            // Senin projende session adı SetInt32("kullaniciId") olduğu için GetInt32 kullanıyoruz
            var kullaniciId = HttpContext.Session.GetInt32("kullaniciId"); 
            
            if (kullaniciId == null)
            {
                return RedirectToAction("Login"); // Giriş yapılmadıysa login sayfasına gönder
            }

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Id == kullaniciId);

            if (kullanici == null) return NotFound();

            return View(kullanici);
        }

        // 2. Form gönderildiğinde verileri güncelleyen metot (POST)
        [HttpPost]
        public IActionResult Ayarlar(Kullanici güncelVeri)
        {
            var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(k => k.Id == güncelVeri.Id);
            if (mevcutKullanici != null)
            {
                mevcutKullanici.Email = güncelVeri.Email;
                mevcutKullanici.Sifre = güncelVeri.Sifre;
                _context.SaveChanges();
                TempData["Mesaj"] = "Hesap bilgileriniz başarıyla güncellendi.";
            }
            return View(mevcutKullanici);
        }
    } 
}