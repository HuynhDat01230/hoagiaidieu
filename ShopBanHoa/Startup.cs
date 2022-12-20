using System;
using System.Collections.Generic;
using System.Linq;
using Owin;
using Microsoft.Owin;
using System.Web;
[assembly: OwinStartup(typeof(ShopBanHoa.Startup))]

namespace ShopBanHoa
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}