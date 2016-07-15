using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhamOnline.Models
{
    public class Constants
    {
        public class DataTypes
        {
            public const string TString = "string";
            public const string TInt = "int";
            public const string TDouble = "double";
            public const string TBool = "bool";
            public const string TDateTime = "DateTime";

            public const string TRef = "Ref"; 

            public static readonly string[] All = new[]
            {
                TString, TInt, TDouble, TBool, TDateTime, TRef
            };
        }
    }
}