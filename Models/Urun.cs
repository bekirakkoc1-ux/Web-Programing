using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Eticaretmvc.Models
{
    public class Urun
    {
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; } = string.Empty;

        [Required]
        public decimal Fiyat { get; set; }
        
        [Required]
        public  string Aciklama { get; set; }

        public string? ResimYolu { get; set; }

        [Required]
        public int KategoriId { get; set; }

        public Kategori? Kategori { get; set; }
    }
}