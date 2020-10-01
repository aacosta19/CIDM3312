using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindMode {

    public static class FindMode {

        public static T? Mode<T>(this IEnumerable<T> source) where T : struct {
            var sortedList = from number in source
                     orderby number
                     select number;

            int count = 0;
            int max = 0;
            T current = default(T);
            T? mode = new T?();

            foreach (T next in sortedList) {
             if (current.Equals(next) == false)
            {
                current = next;
                count = 1;
            }
            else
            {
                count++;
            }

            if (count > max)
            {
                max = count;
                mode = current;
            }
        }

            if (max > 1)
                return mode;

            return null;
        }

    //I was trying to use the same Tuples from the Median Extensions for Mode
     
    //     public static float Mode<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    // {
    //     return source.Select(selector).Mode();
    // }
    
    // public static float? Mode<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    // {
    //     return source.Select(selector).Mode();
    // }

}//End Class

}//End namspace