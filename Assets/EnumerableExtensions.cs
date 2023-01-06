using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions 
{
    public static T Random<T>(this IEnumerable<T> enumerable, T previous) where T : class
    {
        return enumerable.Random(previous, _ => true);
    }
    
    public static T Random<T>(this IEnumerable<T> enumerable, T previous, Func<T, bool> predict) where T : class
    {
        var en = enumerable.ToList();
        var element = en.RandomItem();
        var iterations = 0;

        while (element == null || element.Equals(previous) || predict(element) == false)
        {
            element = en.RandomItem();
            iterations++;
            
            if(iterations >= 100)
                return null;
        }

        return element;
    }

    private static T RandomItem<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}