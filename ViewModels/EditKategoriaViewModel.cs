using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RegionalShop.Models;

namespace RegionalShop.ViewModels
{
    public class EditKategoriaViewModel
    {
        public Kategoria Kategoria { get; set; }
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public bool? Potwierdzenie { get; set; }
    }
}