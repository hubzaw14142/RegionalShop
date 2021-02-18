using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Owin;
using RegionalShop.App_Start;

namespace RegionalShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("ProduktyContext");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}