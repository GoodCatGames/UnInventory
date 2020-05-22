using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample.Trade.Data
{
    public struct BagTableStruct
    {
        public IDataInventoryContainer Bag;
        public IDataInventoryContainer Table;

        public BagTableStruct(IDataInventoryContainer bag, IDataInventoryContainer table)
        {
            Bag = bag;
            Table = table;
        }
    }
}
