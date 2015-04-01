using System;
using DotLiquid;
using System.IO;
using System.Text;

namespace Wham
{
    public class MultilineStringEscape : Block
    {
        public static StringBuilder EscapeAndNewlines(StringBuilder sb)
        {
            sb.Replace("\\", "\\\\");
            sb.Replace("\"", "\\\""); 
            sb.Replace("\r\n", "\"\r\n + \"");
            sb.Insert(0, '"');
            sb.Append('"');
            return sb;
        }

        public override void Render(Context context, System.IO.TextWriter result)
        {  
            using (StringWriter swt = new StringWriter())
            {
                base.Render(context, swt);
                var sb = swt.GetStringBuilder(); 
                EscapeAndNewlines(sb); 
                result.Write(sb.ToString()); 
            } 
        }
    }
}

