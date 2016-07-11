using System;
using DotLiquid;
using System.Linq;

namespace Wham
{
    public static class DotLiquidExtensions
    {
        public static void RegisterSafeType(Type t, Func<object,object> converter = null)
        { 
            var allProps = t.GetProperties().Select(p => p.Name).ToArray();

            if (converter == null)
                converter = o => o;
            
            Template.RegisterSafeType(t, allProps, converter); 
        }

        public static string RenderWithErrors(this Template template, Hash hash)
        {
            RenderParameters prm = new RenderParameters
            {   
                LocalVariables = hash,
                RethrowErrors = true,
            }; 

            var res = template.Render(prm); 
            return res;
        }
    }
}

