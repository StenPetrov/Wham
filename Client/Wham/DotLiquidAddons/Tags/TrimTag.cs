using System;
using DotLiquid;
using System.IO;

namespace Wham
{
    public class TrimTag : Block
    {
        public override void Render(Context context, System.IO.TextWriter result)
        {  
            using (StringWriter swt = new StringWriter())
            {
                base.Render(context, swt);
                var sb = swt.GetStringBuilder(); 
                if (sb.Length > 0)
                {
                    var sr = sb.ToString().Trim();
                    result.Write(sr); 
                }
            } 
        }
    }
}

