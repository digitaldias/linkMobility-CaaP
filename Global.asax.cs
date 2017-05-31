using LogisticBot.IoC;
using StructureMap;
using System.Diagnostics;
using System.Web.Http;

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
