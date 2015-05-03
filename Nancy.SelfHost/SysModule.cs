using System;

namespace Nancy.SelfHost
{
    public class SysModule : NancyModule
    {
        public SysModule()
        {
            Get["/api/sys/version"] = _ =>
            {
                return "SelfHost 1.1";
            };
        }
    }
}

