using System;
using DotLiquid;
using System.Text.RegularExpressions;
using System.Linq;

namespace Wham
{
    /// <summary>
    /// Outputs a debrix code: 10-12 upcase characters in square brackets, 
    /// such as [FJMHRTZFGHL].
    /// </summary>
    public class DebrixTag : Tag
    {
        public override void Render(Context context, System.IO.TextWriter result)
        {

            string debrix = string.Empty;

            while (debrix.Length < 10)
            {
                var guids = Guid.NewGuid().ToByteArray()
                    .Union(Guid.NewGuid().ToByteArray())
                    .Union(Guid.NewGuid().ToByteArray())
                    .Union(Guid.NewGuid().ToByteArray())
                    .Union(Guid.NewGuid().ToByteArray())
                    .Union(Guid.NewGuid().ToByteArray()).ToArray();
                var chars = new char[128];

                var r = Convert.ToBase64CharArray(guids, 0, guids.Length, chars, 0);

                debrix = new string(chars
                    .Where(c => char.IsLetter(c) && "AEOYUIaeoyui".IndexOf(c) < 0)
                    .Select(c => char.ToUpper(c))
                    .Take(12).ToArray());
            }
             
            result.Write("[" + debrix + "]");
        }
    }
}