using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnInventory.Core.Extensions
{
    public static class TransformExt
    {
        public static IEnumerable<T> GetComponentsRootAndChildren<T>(this Transform transform)
            where T : MonoBehaviour
        {
            var result = transform.GetComponentsInChildren<T>();
            return result.Where(component => component != null);
        }
    }
}
