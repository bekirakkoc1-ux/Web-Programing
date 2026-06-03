using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eticaretmvc.Models
{
    public class Siparis
    {
        public int Id { get; set; }

        [Required]
        public int KullaniciId { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        public List<SiparisDetay> Detaylar { get; set; } = new();
    }
}
