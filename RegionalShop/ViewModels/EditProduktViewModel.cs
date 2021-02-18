using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RegionalShop.Models;

namespace RegionalShop.ViewModels
{
    public class EditProduktViewModel
    {
        public Produkt Produkt { get; set; }
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public bool? Potwierdzenie { get; set; }
    }
}