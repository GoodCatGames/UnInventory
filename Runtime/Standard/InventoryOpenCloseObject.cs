using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnInventory.Core.Extensions;
using UnityEngine;
using UnInventory.Core.InventoryBindings;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using Object = UnityEngine.Object;

namespace UnInventory.Standard
{
    public class InventoryOpenCloseObject : IInventoryOpenCloseObject
    {
        private const string NameInventoryDefault = "NoNameInventory";

        public bool IsOpen { get; private set; }
        public DataInventory DataInventory => _inventoryBinding.DataInventory;
        public Transform Transform { get; private set; }

        private InventoryComponent _inventoryComponent;
        private readonly string _nameInventory;
        private readonly GameObject _prefabWithInventory;
        private readonly IInventoryBinding _inventoryBinding;

        private IInventoryBindingFactory InventoryBindingFactory =>
            InventoryManager.ContainerDi.InventoryBindingFactory;

        public InventoryOpenCloseObject(GameObject prefabWithInventory,
            IEnumerable<DataEntity> dataEntitiesForLoad = null,
            string nameInventory = NameInventoryDefault)
        {
            _prefabWithInventory = prefabWithInventory;
            _nameInventory = nameInventory;
            var inventoryComponent = InventoryComponent.GetInventoryComponent(prefabWithInventory.transform);
            CheckInventoryOnScene(inventoryComponent);

            _inventoryBinding = InventoryBindingFactory.Create(inventoryComponent, dataEntitiesForLoad);
            _inventoryBinding.ChangeState.AddListener(state =>
            {
                if (state == State.UnBind)
                {
                    ProcessClose();
                }
            });
        }

        public void OpenClose(Vector2? position = null)
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open(position);
            }
        }

        public void Open(Vector2? position = null)
        {
            var instantiateInventory = InstantiateInventory();
            Transform = instantiateInventory.transform;
            PlaceTransform(position);

            _inventoryComponent = InventoryComponent.GetInventoryComponent(instantiateInventory.transform);
            _inventoryComponent.DataInventory.NameInstance = _nameInventory;

            _inventoryBinding.Bind(_inventoryComponent);
            
            IsOpen = true;
        }

        private void PlaceTransform(Vector2? position)
        {
            if (position != null)
            {
                Transform.localPosition = position.Value;
            }
        }

        public void Close() => _inventoryBinding.UnBind();

        private GameObject InstantiateInventory()
        {
            var instantiateInventory = _prefabWithInventory.InstantiateOnRootCanvas();
            return instantiateInventory;
        }

        private void ProcessClose()
        {
            Object.Destroy(Transform.gameObject);
            IsOpen = false;
        }

        private void CheckInventoryOnScene(InventoryComponent inventoryComponent) =>
            Contract.Assert(!inventoryComponent.OnScene(), "Inventory is already on scene! You should use prefab.");
    }
}