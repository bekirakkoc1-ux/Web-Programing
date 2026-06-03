using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eticaretmvc.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        public string Ad { get; set; } = string.Empty;

        public ICollection<Urun>? Urunler { get; set; }
    }
}
