using System;
using DotLiquid;
using System.Text.RegularExpressions;

namespace Wham
{
    public class GuidTag : Tag
    {
        public string GuidFormat { get; set; } = "D";

        public Regex MarkupRegex = new Regex("^(N|D|B|P|X)$");

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            if (!string.IsNullOrEmpty(markup))
            {
                markup = markup.Trim('\'', '\"', ' ');
                if (MarkupRegex.IsMatch(markup))
                    GuidFormat = markup;
                else
                    throw new WhamException("[OFAIJKEWQRPO] Invalid GUID format: " + markup);
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, System.IO.TextWriter result)
        {
            result.Write(Guid.NewGuid().ToString(GuidFormat).ToUpper());
        }
    }
}