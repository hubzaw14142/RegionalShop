using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using MvcSiteMapProvider;
using RegionalShop.DAL;
using RegionalShop.Models;

namespace RegionalShop.Infrastructure
{
    public class ProduktySzczegolyDynamicNodeProvider : DynamicNodeProviderBase
    {

        private ProduktyContext db = new ProduktyContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodee)
        {

            var returnValue = new List<DynamicNode>();

            foreach(Produkt produkt in db.Produkty)
            {
                DynamicNode node = new DynamicNode();
                node.Title = produkt.NazwaProduktu;
                node.Key = "Produkt_" + produkt.ProduktId;
                node.ParentKey = "Kategoria_" + produkt.KategoriaId;
                node.RouteValues.Add("id", produkt.ProduktId);
                returnValue.Add(node);
            }

            return returnValue;
        }

    }
}