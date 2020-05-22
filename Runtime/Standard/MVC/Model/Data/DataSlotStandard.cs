using System;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Data
{
    [Serializable]
    public class DataSlotStandard : DataSlot
    {
        public override DataSlot Clone() => CloneDataSlot();
    }
}
