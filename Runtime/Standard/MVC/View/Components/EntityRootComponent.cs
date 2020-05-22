using JetBrains.Annotations;
using UnityEngine;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Core.MVC.View.Components.Entity;
using UnInventory.Standard.Configuration;
using UnityEngine.Events;

namespace UnInventory.Standard.MVC.View.Components
{
    public class EntityRootComponent : MonoBehaviour, IEntityRootComponent
    {
        public InventoryComponent InventoryComponent { get; private set; }
        public Transform Transform => transform;
        public RectTransform RectTransform { get; private set; }
        
        public UnityEvent<DataEntity, DataEntity> SetNewDataEvent { get; } = new SetNewDataEntityEvent();

        public DataEntity Data { get; private set; }

        private IBindComponentToDataDbWrite BindComponentToDataDbWrite => InventoryManager.ContainerDiOverride<ContainerDiStandard>().BindComponentToDataDbWrite;

        public void Init(InventoryComponent inventoryComponent, DataEntity dataEntity)
        {
            InventoryComponent = inventoryComponent;
            BindComponentToDataDbWrite.BindEntity(this);
            SetData(dataEntity);
        }

        [UsedImplicitly]
        public void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void SetData(DataEntity data)
        {
            var oldDataEntity = Data;
            Data = data;
            SetNewDataEvent.Invoke(oldDataEntity, Data);
        }

        [UsedImplicitly]
        public void OnDestroy()
        {
            BindComponentToDataDbWrite.UnBindEntity(this);
        }

        private class SetNewDataEntityEvent : UnityEvent<DataEntity, DataEntity>
        {
        }
    }
}
