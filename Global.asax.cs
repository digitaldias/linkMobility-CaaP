using LogisticBot.IoC;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace LogisticBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Container IoCResolver = new Container(new RuntimeRegistry());


        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

#if DEBUG
            Trace.WriteLine(IoCResolver.WhatDidIScan());
            Trace.WriteLine(IoCResolver.WhatDoIHave());
#endif

        }
    }
}
