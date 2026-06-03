using System.ComponentModel.DataAnnotations;

namespace Eticaretmvc.Models
{
    public class SiparisDetay
    {
        public int Id { get; set; }

        [Required]
        public int SiparisId { get; set; }

        [Required]
        public int UrunId { get; set; }

        public int Adet { get; set; }
        public decimal Fiyat { get; set; }

        public Urun? Urun { get; set; }
    }
}
