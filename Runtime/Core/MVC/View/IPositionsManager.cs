using UnityEngine;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Entity;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Core.MVC.View
{
    public interface IPositionsManager
    {
        void PlaceEntityTransform(IEntityRootComponent entity, ISlotRootComponent slot);

        /// <summary>
        /// Returns Vector2 which must be added to the center coordinates of entityArea to get the center of LeftTopSlot
        /// </summary>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        Vector2 GetDeltaEntityAreaCenterToLeftTopWithScaleFactor(DataEntity dataEntity);
    }
}