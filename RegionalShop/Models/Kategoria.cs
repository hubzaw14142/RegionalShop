using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegionalShop.Models
{
    public class Kategoria
    {
        public int KategoriaId { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwe kategorii")]
        [StringLength(50)]
        public string NazwaKategorii { get; set; }

        [StringLength(100)]
        public string OpisKategorii { get; set; }
        public string NazwaPlikuIkony { get; set; }
        public bool Ukryta { get; set; }
        public virtual ICollection<Produkt> Produkty { get; set; }
    }
}