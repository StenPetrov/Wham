using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace Wham.DotLiquidAddons.Filters
{
    public static class ArithmeticFilters
    {
        private static readonly double Tolerance = Double.Epsilon*100;

        public static object Add(object input, object parameter, Context context = null)
        {
            double inp, prm;
            GetArithmeticParameters(input, parameter, out inp, out prm);

            double res = inp + prm;

            if (res - Math.Round(res) <= Tolerance)
                return (int)res;

            return res;
        }
         
        public static object Sub(object input, object parameter, Context context = null)
        {
            double inp, prm;
            GetArithmeticParameters(input, parameter, out inp, out prm);

            double res = inp - prm;

            if (res - Math.Round(res) <= Tolerance)
                return (int)res;

            return res;
        }

        public static object Mult(object input, object parameter, Context context = null)
        {
            double inp, prm;
            GetArithmeticParameters(input, parameter, out inp, out prm);

            double res = inp * prm;

            if (res - Math.Round(res) < Tolerance)
                return (int)res;

            return res;
        }
         
        public static object Div(object input, object parameter, Context context = null)
        {
            double inp, prm;
            GetArithmeticParameters(input, parameter, out inp, out prm);

            if (Math.Abs(prm) <= Tolerance)  
                return "[AIQJNHTRHNJD] Parameter for division can't be 0";

            double res = inp / prm;

            if (res - Math.Round(res) <= Tolerance)
                return (int)res;

            return res;
        }

        private static void GetArithmeticParameters(object input, object parameter, out double inp, out double prm)
        {
            inp = (double) Convert.ChangeType(input, typeof (double));
            if (parameter is string)
            {
                parameter = ((string) parameter).Trim('\'');
            }
            prm = (double) Convert.ChangeType(parameter, typeof (double)); 
        }
    }
}
