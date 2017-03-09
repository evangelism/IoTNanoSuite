using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceEmulator
{
    public static class Utils
    {

        public static float ParseFloat(this string s)
        {
            try
            {
                return float.Parse(s.Replace(',','.'),NumberStyles.Float);
            }
            catch
            {
                return float.NaN;
            }
        }
    }
}
