using System.Collections.Generic;
using UnityEngine;

namespace UnInventory.Core.Extensions
{
    public static class Vector2IntExtension
    {
        public static IEnumerable<Vector2Int> GetArea(Vector2Int minVector, Vector2Int maxVector)
        {
            var result = new List<Vector2Int>();
            for (var x = minVector.x; x <= maxVector.x; x++)
            {
                for (var y = minVector.y; y <= maxVector.y; y++)
                {
                    result.Add(new Vector2Int(x, y));
                }
            }
            return result;
        }
    }
}
