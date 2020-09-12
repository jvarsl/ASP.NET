using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Tools
{
    public class DayToWord
    {
        private static readonly Dictionary<int, string> dayDic = new Dictionary<int, string>()
        {
            {1, "Pirmadienis"},
            {2, "Antradienis"},
            {3, "Trečiadienis"},
            {4, "Ketvirtadienis"},
            {5, "Penktadienis"},
            {6, "Šeštadienis"},
            {7, "Sekmadienis"}
        };

        public static string LithuanianDayWord(byte? day)
        {
            if (day.HasValue) return dayDic.GetValueOrDefault(Convert.ToInt32(day.Value));
            else return "";
        }
    }
}
