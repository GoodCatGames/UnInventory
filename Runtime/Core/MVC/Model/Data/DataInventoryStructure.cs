using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;

namespace UnInventory.Core.MVC.Model.Data
{
    public class DataInventoryStructure : ICloneable
    {
        [NotNull] public readonly DataInventory DataInventory;

        public IReadOnlyCollection<DataSlot> Slots => _slots;
        private readonly HashSet<DataSlot> _slots;

        public DataInventoryStructure([NotNull] DataInventory dataInventory, [NotNull] IEnumerable<DataSlot> slots)
        {
            DataInventory = dataInventory;
            _slots = new HashSet<DataSlot>(slots);
            Contract.Assert(_slots.Any(), "Collection should not be empty!");
            Contract.Assert(_slots.Select(slot => slot.GetType()).Distinct().Count() == 1, "Must be the only one type of slot in the collection");
        }

        object ICloneable.Clone() => Clone();
        
        public DataInventoryStructure Clone()
        {
            var result = new List<DataSlot>();
            var dataInventoryClone = DataInventory.Clone();
            foreach (var dataSlot in _slots)
            {
                var slotClone = dataSlot.Clone();
                ((IDataInventorySetter)slotClone).SetDataInventory(dataInventoryClone);
                result.Add(slotClone);
            }
            return new DataInventoryStructure(dataInventoryClone, result);
        }
    }
}
