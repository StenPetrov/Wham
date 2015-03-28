using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace Wham
{
    public static class CollectionFilters
    {
        public static IEnumerable<object> Except(object input, string filterOut)
        {
            IEnumerable enu = input as IEnumerable;
            if (enu != null)
            {
                Regex filter = new Regex(filterOut);

                IEnumerable<object> coll = enu.OfType<object>();
                coll = coll.Where(o => o != null && !filter.IsMatch(o.ToString()));
                return coll;
            } 

            return null; 
        }
    }
}

