using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnInventory.Core.Extensions;
using UnInventory.Core.Manager;
using UnityEngine.Serialization;

namespace UnInventory.Core.MVC.Model.Data
{
    [Serializable]
    public abstract class DataEntity : ScriptableObject, IData, IDataInventorySetter
    {
        public static DataEntity Create() => (DataEntity) CreateInstance(typeof(DataEntity));

        public DataInventory DataInventory { get; private set; }
        public bool IsFull => Amount == AmountMax;

        /// <inheritdoc />
        /// <summary>
        /// Should be Invoke if you change data
        /// </summary>
        public UnityEvent DataChangeEvent { get; } = new UnityEvent();

        public Sprite Sprite => sprite;

        [FormerlySerializedAs("Sprite")] [SerializeField]
        private Sprite sprite = default;

        public string Id
        {
            get => string.IsNullOrEmpty(_id) ? Sprite.name : null;
            protected set => _id = value;
        }

        private string _id;

        public bool IsMayBeStack => AmountMax != 1;

        // The maximum number of pieces in one stack
        public int AmountMax
        {
            get => _amountMax;
            set => _amountMax = value < 1 ? 1 : value;
        }

        [SerializeField, Min(1)] private int _amountMax = 1;

        public int Amount
        {
            get => _amount;
            set
            {
                var newValue = value < 0 ? 0 : value;
                RxSimple.SetValueInvokeChangeEvent(ref _amount, newValue, DataChangeEvent);
            }
        }

        [SerializeField, Min(0)] private int _amount = 1;


        public bool IsAreaEntity => !_dimensions.Equals(new Vector2Int(1, 1));

        public Vector2Int Dimensions => _dimensions;

        [SerializeField, Header("Width*Height")]
        private Vector2Int _dimensions = new Vector2Int(1, 1);

        public DataEntity Copy()
        {
            var data = Instantiate(this);
            data.name = name;
            return data;
        }

        public IEnumerable<Vector2Int> GetArea(Vector2Int slotLeftTop)
        {
            var result = new HashSet<Vector2Int>();
            for (var x = 0; x < _dimensions.x; x++)
            {
                for (var y = 0; y < _dimensions.y; y++)
                {
                    result.Add(new Vector2Int(slotLeftTop.x + x, slotLeftTop.y + y));
                }
            }

            return result;
        }

        void IDataInventorySetter.SetDataInventory([NotNull] DataInventory dataInventory) =>
            DataInventory = dataInventory;

        public override string ToString()
        {
            var slot = InventoryManager.ContainerDi.DatabaseReadOnly.GetSlotOrNull(this);
            return $"{Id}({slot})";
        }
    }
}