using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.DataBase
{
    internal class ComparerSlots : IComparer<DataSlot>
    {
        public int Compare(DataSlot x, DataSlot y)
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

            int compareRow = x.Row.CompareTo(y.Row);
            int compareColumn = x.Column.CompareTo(y.Column);
            if (compareRow != 0)
            {
                return compareRow;
            }
            else if(compareColumn != 0)
            {
                return compareColumn;
            }

            return 0;
        }
    }
}