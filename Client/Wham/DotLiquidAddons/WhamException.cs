using System;
using DotLiquid.Exceptions;

namespace Wham
{
    public class WhamException : LiquidException
    {
        public WhamException (string message, Exception innerException)
            : base (message, innerException)
        {
        }

        public WhamException (string message)
            : base (message)
        {
        }
    }

    public class WhamTemplateException : WhamException
    {
        public WhamTemplateException (string message, Exception innerException)
            : base (message, innerException)
        {
        }

        public WhamTemplateException (string message)
            : base (message)
        {
        }
    }
}

