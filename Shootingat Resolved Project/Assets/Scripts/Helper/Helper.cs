using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Helper
{
    public static class Helper

    {



        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T RemoveRandomElement<T>(this List<T> list)
        {
            int elementIndex = Random.Range(0, list.Count);
            T value = list[elementIndex];
            list.RemoveAt(elementIndex);
            return value;
        }

        
    }
}
