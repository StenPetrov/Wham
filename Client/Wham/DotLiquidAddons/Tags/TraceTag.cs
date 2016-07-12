using System;
using System.Linq;
using DotLiquid;

namespace Wham
{
    public class TraceTag : Tag
    {
        private string Message = null;

        public TraceTag ()
        {
        }

        public override void Initialize (string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            Message = "[TRACETAG] " + markup + tokens.FirstOrDefault ();
            base.Initialize (tagName, markup, tokens);
        }

        public override void Render (Context context, System.IO.TextWriter result)
        {
            context.GetTracer ().Info (Message);
        }
    }
}

