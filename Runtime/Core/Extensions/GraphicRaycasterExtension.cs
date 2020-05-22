using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnInventory.Core.Extensions
{
    public static class GraphicRaycasterExtension
    {
        public static IEnumerable<T> GetComponentsInPositionAllCanvas<T>(Vector2 position) 
        {
            var pointerEventData = new PointerEventData(EventSystem.current) {position = position};

            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            var resultsSlots = new List<T>();
            IEnumerable<T> slots = raycastResults.Select(result => result.gameObject.GetComponent<T>()).Where(slot => slot != null).ToList();
            resultsSlots.AddRange(slots);
            return resultsSlots;
        }
    }
}
