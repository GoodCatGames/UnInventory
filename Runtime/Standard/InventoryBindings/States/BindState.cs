using System;
using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Standard.InventoryBindings.States
{
    internal class BindState : InventoryBinding.StateInventoryBind
    {
        internal override DataInventory DataInventory { get; }

        internal BindState(InventoryBinding inventoryDataBind, DataInventory dataInventory) : base(inventoryDataBind)
        {
            DataInventory = dataInventory;
        }

        public override void Bind(InventoryComponent inventoryComponent)
        {
            throw new Exception("Already bind!");
        }

        public override void Unbind(InventoryComponent inventoryComponent)
        {
            var entities = BindComponentToDataDbRead.GetEntityComponents(DataInventory);
            DestroyEntities(entities);
            UnBindInventoryComponent(inventoryComponent);
            BindComponentToDataDbWrite.UnBindInventory(inventoryComponent);
            SetState(new UnbindState(InventoryBinding, DataInventory));
        }

        private void DestroyEntities(IEnumerable<IEntityRootComponent> entities)
        {
            foreach (var entity in entities)
            {
                UnityEngine.Object.Destroy(entity.Transform.gameObject);
            }
        }
    }
}
