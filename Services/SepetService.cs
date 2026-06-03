using System.Collections.Generic;
using System.Linq;
using Eticaretmvc.Extensions;
using Eticaretmvc.Models;
using Microsoft.AspNetCore.Http;

namespace Eticaretmvc.Services
{
    public class SepetService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SEPET_KEY = "Sepet";

        public SepetService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private List<SepetItem> SepetiGetir()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            var sepet = session.GetObject<List<SepetItem>>(SEPET_KEY);
            return sepet ?? new List<SepetItem>();
        }

        private void SepetiKaydet(List<SepetItem> sepet)
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            session.SetObject(SEPET_KEY, sepet);
        }

        public void SepeteEkle(Urun urun, int adet = 1)
        {
            var sepet = SepetiGetir();
            var mevcut = sepet.FirstOrDefault(x => x.Urun.Id == urun.Id);
            if (mevcut != null)
            {
                mevcut.Adet += adet;
            }
            else
            {
                sepet.Add(new SepetItem { Urun = urun, UrunId = urun.Id, Adet = adet });
            }
            SepetiKaydet(sepet);
        }

        public void SepettenSil(int urunId)
        {
            var sepet = SepetiGetir();
            var item = sepet.FirstOrDefault(x => x.UrunId == urunId);
            if (item != null)
            {
                sepet.Remove(item);
                SepetiKaydet(sepet);
            }
        }

        public List<SepetItem> SepetiListele()
        {
            return SepetiGetir();
        }

        public void SepetiTemizle()
        {
            SepetiKaydet(new List<SepetItem>());
        }
    }
}
