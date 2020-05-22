using System;
using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.DataBase
{
    internal class ComparerEntitiesForComparerSlots : IComparer<DataEntity>
    {
        private readonly IComparer<DataSlot> _comparerSlots;
        private readonly DataBase _dataBase;

        public ComparerEntitiesForComparerSlots(DataBase dataBase, IComparer<DataSlot> comparerSlots)
        {
            _comparerSlots = comparerSlots;
            _dataBase = dataBase;
        }

        public int Compare(DataEntity x, DataEntity y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return 1;
            }

            if (y == null)
            {
                return -1;
            }

            var slotEntityX = _dataBase.GetSlotOrNull(x);
            var slotEntityY = _dataBase.GetSlotOrNull(y);

            if(slotEntityX == null || slotEntityY == null) { throw new Exception(); }
            
            return _comparerSlots.Compare(slotEntityX, slotEntityY);
        }
    }
}