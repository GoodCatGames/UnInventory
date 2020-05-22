using JetBrains.Annotations;
using UnityEngine;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnityEngine.Events;

namespace UnInventory.Core.MVC.View.Components.Slot
{
    public abstract class SlotRootComponent<T> : MonoBehaviour, ISlotRootComponent
        where T : DataSlot, new()
    {
        public Transform Transform => transform;
        public RectTransform RectTransform { get; private set; }

        public UnityEvent<DataSlot, DataSlot> SetNewDataEvent{ get; } = new SetNewDataSlotEvent();

        public DataSlot Data => _dataSlotConcrete;
        [SerializeField] private T _dataSlotConcrete = new T();

        public bool IsEmpty => DatabaseReadOnly == null || DatabaseReadOnly.SlotIsFree(Data);
        public InventoryComponent InventoryComponent { get; private set; }
        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;

        [UsedImplicitly]
        public void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void SetInventory(InventoryComponent inventoryComponent, DataInventory dataInventory)
        {
            InventoryComponent = inventoryComponent;
            ((IDataInventorySetter) Data).SetDataInventory(dataInventory);
        }

        public void SetData(DataSlot dataSlot)
        {
            var oldDataSlot = _dataSlotConcrete;
            _dataSlotConcrete = (T)dataSlot;
            SetNewDataEvent.Invoke(oldDataSlot, _dataSlotConcrete);
            //Debug.Log($"{oldDataSlot} => {_dataSlotConcrete})");
        }

        public override string ToString()
        {
            return $"{InventoryComponent.name} {Data}";
        }

        private class SetNewDataSlotEvent : UnityEvent<DataSlot, DataSlot>
        {
        }
    }
}
