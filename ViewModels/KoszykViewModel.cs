using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RegionalShop.Models;

namespace RegionalShop.ViewModels
{
    public class KoszykViewModel
    {
        public List<PozycjaKoszyka> PozycjeKoszyka { get; set; }
        public decimal CenaCalkowita { get; set; }

    }
}