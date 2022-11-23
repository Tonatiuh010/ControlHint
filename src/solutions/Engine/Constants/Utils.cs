using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Constants
{
    public static class Utils
    {
        public static byte[] GetImage(string url)
        {
            byte[] imageBytes;

            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync(url))
                    {
                        response.Wait();
                        var stream = response.Result.Content.ReadAsStream();
                        using (var memo = new MemoryStream())
                        {
                            stream.CopyTo(memo);
                            imageBytes = memo.ToArray();
                            memo.Dispose();
                            stream.Dispose();
                        }
                    }
                }
            }
            catch
            {
                imageBytes = Array.Empty<byte>();
            }

            return imageBytes;
        }

        public static bool IsNumeric(object obj)
        {
            if (obj == null) return false;

            switch (obj)
            {
                case sbyte: return true;
                case byte: return true;
                case short: return true;
                case ushort: return true;
                case int: return true;
                case uint: return true;
                case long: return true;
                case ulong: return true;
                case float: return true;
                case double: return true;
                case decimal: return true;
            }

            if (obj != null )
            {
                string s = Convert.ToString(obj, CultureInfo.InvariantCulture);

                return double.TryParse(s, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double _);
            }
            else throw new Exception("Something going on checking value!! Utils.IsNumeric()");
        }
        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
