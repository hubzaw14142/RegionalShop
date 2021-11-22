using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegionalShop.Models
{
    public class Produkt
    {
        public int ProduktId { get; set; }
        public int KategoriaId { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwe produktu")]
        [StringLength(50, MinimumLength=4)]
        public string NazwaProduktu { get; set; }
        [StringLength(100)]
        public string NazwaPlikuObrazka { get; set; }
        [Required(ErrorMessage = "Wprowadz opis produktu")]
        [StringLength(500,MinimumLength=10)]
        public string OpisProduktu { get; set; }
        [Required(ErrorMessage = "Wprowadz cene produktu")]
        public decimal CenaProduktu { get; set; }
        public bool DostepnyProdukt { get; set; }
        public string OpisSkrocony { get; set; }
        public bool Bestseller { get; set; }
        public virtual Kategoria Kategoria { get; set; }
        public bool Ukryty { get; set; }
        public DateTime DataDodania { get; set; }

    }
}