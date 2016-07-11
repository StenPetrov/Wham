using System;
using Nancy;
using WhamBase; 
using Wham.Base;

namespace Nancy.SelfHost
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public Bootstrapper()
            : base()
        { 
        }

        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
        }

        protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }
    }
}

