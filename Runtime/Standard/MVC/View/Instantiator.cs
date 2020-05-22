using System;
using UnInventory.Core.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Core.Configuration;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View;
using UnInventory.Core.MVC.View.Components.Entity;
using UnInventory.Standard.Configuration;
using UnInventory.Standard.MVC.View.Components;

namespace UnInventory.Standard.MVC.View
{
    internal class Instantiator : IInstantiator
    {
        private IBindComponentToDataDbRead _bindComponentToDataDbRead => InventoryManager.ContainerDi.
            BindComponentToDataDbRead;
        private readonly IFactoryTypeToPrefab _factoryDataEntityPrefab;

        public Instantiator(IFactoryTypeToPrefab factoryDataEntityPrefab)
        {
            _factoryDataEntityPrefab = factoryDataEntityPrefab;
        }

        public IEntityRootComponent AddEntity(DataEntity dataEntity, DataSlot slotLeftTop)
        {
            var inventory = slotLeftTop.DataInventory;
            var transform = CreateTransform(inventory, dataEntity);
            var entity = transform.GetComponent<EntityRootComponent>();
 
            var inventoryComponent = _bindComponentToDataDbRead.GetInventoryComponent(slotLeftTop.DataInventory);
            entity.Init(inventoryComponent, dataEntity);
            return entity;
        }

        private Transform CreateTransform(DataInventory dataInventory, DataEntity dataEntity)
        {
            var inventoryComponent = _bindComponentToDataDbRead.GetInventoryComponent(dataInventory);
            var prefab = _factoryDataEntityPrefab.GetPrefab(dataEntity.GetType());
            var entityTransform = prefab.InstantiateOnRootCanvas().transform;
            
            entityTransform.SetParent(inventoryComponent.CanvasInventory.transform);

            var sprite = dataEntity.Sprite;
            if (sprite == null)
            {
                throw new NullReferenceException($"{this}: Sprite cannot be null!");
            }
            var image = entityTransform.gameObject.GetComponentInChildren<Image>();
            image.sprite = sprite;
            image.raycastTarget = false;
           
            return entityTransform;
        }
    }
}
