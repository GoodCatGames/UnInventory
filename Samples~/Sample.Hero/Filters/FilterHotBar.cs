using System;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Filters
{
    public class FilterHotBar :
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    {
        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly; 
        [NotNull] private readonly IDataInventoryContainer _inventoryObject;

        public FilterHotBar([NotNull] IDataInventoryContainer inventoryOpenCloseObject)
        {
            _inventoryObject = inventoryOpenCloseObject ??
                               throw new ArgumentNullException(nameof(inventoryOpenCloseObject));
        }


        public bool Validate(MoveInputData data)
        {
            return OnlyForIsHotBarItem(data.ToInventory, data.EntitySource)
                   && NoMoveFromInventory(data.FromInventory, data.ToInventory)
                   && NoMoveInHotBarIfHaveSameId(data.FromInventory, data.ToInventory, data.EntitySource);
        }

        public bool Validate(StackInputData data)
        {
            return InStackDenied(data.ToInventory) && InStackDenied(data.FromInventory);
        }

        public bool Validate(SwapPrimaryInputData data)
        {
            return OnlyForIsHotBarItem(data.ToInventory, data.EntitySource) &&
                   NoMoveFromInventory(data.FromInventory, data.ToInventory)
                   && NoMoveInHotBarIfHaveSameId(data.FromInventory, data.ToInventory, data.EntitySource);
        }

        /// <summary>
        /// StackDenied (In, From)
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        private bool InStackDenied(DataInventory inventory) => inventory != _inventoryObject.DataInventory;

        /// <summary>
        /// DataEntityEquipment.IsHotBarItem == true
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        private bool OnlyForIsHotBarItem(DataInventory inventory, DataEntity dataEntity)
        {
            return inventory != _inventoryObject.DataInventory ||
                   ((dataEntity is DataEntityEquipment dataEntityEquipment) &&
                    dataEntityEquipment.IsHotBarItem);
        }

        private bool NoMoveFromInventory(DataInventory inventoryFrom, DataInventory inventoryTo)
        {
            return inventoryFrom != _inventoryObject.DataInventory || inventoryTo == _inventoryObject.DataInventory;
        }

        private bool NoMoveInHotBarIfHaveSameId(DataInventory inventoryFrom, DataInventory inventoryTo, DataEntity dataEntity)
        {
            if (inventoryTo == inventoryFrom)
            {
                return true;
            }
            var ids = DatabaseReadOnly.GetDataEntitiesInventory(inventoryTo).Select(data => data.Id).Distinct();
            return !(inventoryTo == _inventoryObject.DataInventory && ids.Contains(dataEntity.Id));
        }
    }
}