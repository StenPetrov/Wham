using System;
using DotLiquid;
using System.Linq;

namespace Wham
{
    public static class DotLiquidExtensions
    {
        public static void RegisterSafeType(Type t)
        { 
            var allProps = t.GetProperties().Select(p => p.Name).ToArray();

            Template.RegisterSafeType(t, allProps, o => o); 
        }
    }
}

