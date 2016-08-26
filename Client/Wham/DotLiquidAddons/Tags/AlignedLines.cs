using System;
using DotLiquid;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wham
{
    public class AlignedLines : Block
    {
        private int maxBlankLines = 0;
        private bool trim = true;
        private bool align = true;
        private string replace = "[onespace]";
        private string replacement = " ";

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            if (!string.IsNullOrEmpty(markup))
            {
                if (!string.IsNullOrWhiteSpace(markup))
                {
                    string markdown = markup.Trim().Trim('\'').ToLower();

                    if (markdown.IndexOf("singleblanklines", StringComparison.Ordinal) >= 0)
                        maxBlankLines = 1;
                    else if (markdown.IndexOf("noblanklines", StringComparison.Ordinal) >= 0)
                        maxBlankLines = 0;

                    if (markdown.IndexOf("notrim", StringComparison.Ordinal) >= 0)
                        trim = false;
                    else if (markdown.IndexOf("trim", StringComparison.Ordinal) >= 0)
                        trim = true;

                    if (markdown.IndexOf("noalign", StringComparison.Ordinal) >= 0)
                        align = false;
                     
                    int eqIdx = markdown.IndexOf("=", StringComparison.Ordinal);
                    if (eqIdx > 0 && eqIdx < markdown.Length - 1)
                    {
                        int eqEnd = markdown.IndexOf(' ', eqIdx);
                        if (eqEnd < eqIdx)
                            eqEnd = markdown.Length - 1;

                        replace = markdown[eqIdx - 1].ToString();
                        replacement = markdown.Substring(eqIdx + 1, eqEnd - eqIdx)
                            .Trim('\'').Replace("\\t", "\t").Replace("\\n", "\n").Replace("\\r", "\r");
                        int repSpace;
                        if (Int32.TryParse(replacement, out repSpace))
                        {
                            replacement = new string(' ', repSpace);
                        }
                    }
                }
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
                    var sr = Align(sb.ToString(), maxBlankLines, trim, align);
                    if (replacement != null && replace != null)
                    {
                        sr = sr.Replace(replace, replacement);
                    }
                    result.Write(sr);
                }
            }
        }
         
        public static string Align(string sb, int maxBlankLines = 0, bool trim = true, bool align = true)
        {
            string blankLines = maxBlankLines > 0 ? string.Join("", Enumerable.Range(1, maxBlankLines).Select(s => Environment.NewLine)) : string.Empty;
            var sr = Regex.Replace(sb, @"^\s+$[\r\n]*", blankLines, RegexOptions.Multiline);
            if (align && sr.Length > 1 && !string.IsNullOrWhiteSpace(sr) && Char.IsWhiteSpace(sr[0]))
            {
                var firstBlank = Regex.Match(sr, @"^\s+");
                sr = Regex.Replace(sr, @"^[^\S\r\n]+", firstBlank.Value, RegexOptions.Multiline); // replace line-leading whitespaces except empty lines
            }
            return trim && sr != null ? sr.Trim() : sr;
        }
    }
}

