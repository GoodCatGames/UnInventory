using System.Linq;
using UnityEngine;

namespace UnInventory.Core.Extensions
{
    public static class MonoBehaviourExt
    {
        public static bool OnScene<T>(this T component)
            where T : MonoBehaviour
        {
            return Object.FindObjectsOfType<T>().ToList().Contains(component);
        }
    }
}
