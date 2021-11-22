using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using RegionalShop.DAL;
using RegionalShop.Models;

namespace RegionalShop.Infrastructure
{
    public class KategorieDynamicNodeProvider : DynamicNodeProviderBase
    {

        public ProduktyContext db = new ProduktyContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodee)
        {

            var returnValue = new List<DynamicNode>();

            foreach (Kategoria kategoria in db.Kategorie)
            {
                DynamicNode node = new DynamicNode();
                node.Title = kategoria.NazwaKategorii;
                node.Key = "Kategoria_" + kategoria.KategoriaId;
                node.RouteValues.Add("nazwaKategorii", kategoria.NazwaKategorii);
                returnValue.Add(node);
            }

            return returnValue;
        }

    }

}