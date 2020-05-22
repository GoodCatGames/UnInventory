using UnityEngine.EventSystems;

namespace UnInventory.Core.Extensions
{
    public static class PointerEventDataExtension
    {
        private const int LeftMouseClickPointerId = -1;
        private const int RightMouseClickPointerId = -2;
        private const int CenterMouseClickPointerId = -3;

        public static bool PointerIdIsLeftMouse(this PointerEventData eventData) => eventData.pointerId == LeftMouseClickPointerId;
        public static bool PointerIdIsRightMouse(this PointerEventData eventData) => eventData.pointerId == RightMouseClickPointerId;
        public static bool PointerIdIsCenterMouse(this PointerEventData eventData) => eventData.pointerId == CenterMouseClickPointerId;
    }
}
