using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Standard.MVC.Model.Commands.Composite;
using UnInventory.Standard.MVC.Model.Commands.Composite.InputData;

namespace UnInventory.Standard.InventoryBindings.States
{
    internal class NoInitState : InventoryBinding.StateInventoryBind
    {
        internal override DataInventory DataInventory => _inventoryStructure.DataInventory;

        protected internal ICommandsFactory CommandsFactory => InventoryManager.ContainerDi.Commands;

        private readonly IEnumerable<DataEntity> _dataEntitiesForLoad;
        private readonly DataInventoryStructure _inventoryStructure;

        
        public NoInitState(InventoryBinding inventoryDataBind, DataInventoryStructure inventoryStructure,
            IEnumerable<DataEntity> dataEntitiesForLoad = null) : base(inventoryDataBind)
        {
            _inventoryStructure = inventoryStructure;
            _dataEntitiesForLoad = dataEntitiesForLoad;
        }

        public override void Bind(InventoryComponent inventoryComponent)
        {
            BindInventoryComponent(inventoryComponent);
            BindComponentToDataDbWrite.RegisterInventoryInDatabase(_inventoryStructure, inventoryComponent);
            SetState(new BindState(InventoryBinding, DataInventory));
            LoadIfNecessary(DataInventory);
        }

        public override void Unbind(InventoryComponent inventoryComponent)
        {
            throw new Exception($"Can t Unbind {typeof(NoInitState)}");
        }

        private void LoadIfNecessary(DataInventory dataInventory)
        {
            if (_dataEntitiesForLoad == null || !_dataEntitiesForLoad.Any()) { return; }
            
            var command = CommandsFactory.Create<CreateEntitiesCommand>()
                .EnterData(new CreateEntitiesInputData(dataInventory, _dataEntitiesForLoad));
            var executeTry = command.ExecuteTry();
            if (!executeTry)
            {
                Debug.Log(command.GetCausesFailure());
                throw new Exception();
            }
        }
    }
}
