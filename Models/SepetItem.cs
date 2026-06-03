namespace Eticaretmvc.Models
{
    public class SepetItem
    {
        public int UrunId { get; set; }
        public Urun Urun { get; set; } = null!;
        public int Adet { get; set; }
    }
}
