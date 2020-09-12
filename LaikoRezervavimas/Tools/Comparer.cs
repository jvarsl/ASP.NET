using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Tools
{
    public class Comparer
    {
        public static bool IsDefaultValue<T>(T obj)
        {
            if (EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                return true;
            }
            return false;
        }
    }
}
