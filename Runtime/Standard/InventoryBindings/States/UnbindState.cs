using System;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;

namespace UnInventory.Standard.InventoryBindings.States
{
    internal class UnbindState : InventoryBinding.StateInventoryBind
    {
        internal override DataInventory DataInventory { get; }
        
        public UnbindState(InventoryBinding inventoryDataBind, DataInventory dataInventory) : base(inventoryDataBind)
        {
            DataInventory = dataInventory;
        }

        public override void Bind(InventoryComponent inventoryComponent)
        {
            BindInventoryComponent(inventoryComponent);
            BindComponentToDataDbWrite.BindInventory(inventoryComponent, DataInventory);
            SetState(new BindState(InventoryBinding, DataInventory));
        }

        public override void Unbind(InventoryComponent inventoryComponent)
        {
            throw new Exception("Already Unbind");
        }
    }
}
