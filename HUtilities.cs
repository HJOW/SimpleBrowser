using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleExplorer
{
    public class HUtilities
    {
        public static Boolean isEmpty(object obj)
        {
            if (obj == null) return true;

            string str = obj.ToString();
            str = str.Trim();
            if (str.Length <= 0) return true;
            return false;
        }
        public static System.Diagnostics.Process RunProgram(string str)
        {
            return System.Diagnostics.Process.Start(str);
        }
        public static System.Diagnostics.Process RunProgram(string str, string param)
        {
            return System.Diagnostics.Process.Start(str, param);
        }
    }
}
