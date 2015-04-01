using System;
using DotLiquid;
using System.IO;
using System.Text;

namespace Wham
{
    public class MultilineStringEscape : Block
    {
        public static StringBuilder EscapeAndNewlines(StringBuilder sb, bool trim = true)
        {
            if (trim)
            {
                int pre = 0;
                while (pre < sb.Length && Char.IsWhiteSpace(sb[pre]))
                    pre++;
                if (pre > 0)
                    sb.Remove(0, pre);

                if (sb.Length > 0)
                {
                    pre = 0;
                    while (Char.IsWhiteSpace(sb[sb.Length - pre - 1]))
                        pre++;
                    if (pre > 0)
                        sb.Remove(sb.Length - pre, pre);
                }
            }

            sb.Replace("\\", "\\\\");
            sb.Replace("\"", "\\\""); 
            sb.Replace("\r\n", "\n");
            sb.Replace("\n", "\"\n + \"");
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

