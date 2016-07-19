using System;
using DotLiquid;
using System.IO;
using System.Text.RegularExpressions;

namespace Wham
{
    public class SingleLineTag : Block
    {
        private bool singleSpace = false;
        private bool noSpace = false;

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            if (!string.IsNullOrEmpty(markup))
            {
                if (string.IsNullOrWhiteSpace(markup))
                    singleSpace = false;
                else if (markup.ToLower() == "singlespace")
                    singleSpace = true;
                else if (markup.ToLower() == "nospace")
                    noSpace = true;
                else
                    throw new WhamException("[AIIQJWRNKRAL] Invalid SingleLine markup: " + markup + ". It must be empty or 'SingleSpace' or 'NoSpace'");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, System.IO.TextWriter result)
        {

            using (StringWriter swt = new StringWriter())
            {
                base.Render(context, swt);
                var sb = swt.GetStringBuilder();
                if (sb.Length > 0)
                {
                    var sr = sb.ToString().Trim().Replace("\r\n", string.Empty).Replace("\n", string.Empty);
                    if (singleSpace || noSpace)
                    {
                        sr = Regex.Replace(sr, @"(?m)\s+", singleSpace ? " " : "");
                    }

                    result.Write(sr);
                }
            }
        }
    }
}

