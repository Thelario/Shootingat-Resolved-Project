using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Helper
{
    public static class Helper

    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default;
            return list[Random.Range(0, list.Count)];
        }

        public static T RemoveRandomElement<T>(this List<T> list)
        {
            int elementIndex = Random.Range(0, list.Count);
            T value = list[elementIndex];
            list.RemoveAt(elementIndex);
            return value;
        }

        public static IEnumerable<Vector2Int> AllNeighbourPositions(this Vector2Int pos)
        {
            yield return pos + Vector2Int.up;
            yield return pos + Vector2Int.right;
            yield return pos + Vector2Int.down;
            yield return pos + Vector2Int.left;
        }
    }
}