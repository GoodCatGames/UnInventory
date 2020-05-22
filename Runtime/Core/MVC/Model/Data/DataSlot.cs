using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnInventory.Core.MVC.Model.Data
{
    [Serializable]
    public abstract class DataSlot : IData, IDataInventorySetter, ICloneable
    {
        public Vector2Int Vector2Int => new Vector2Int(Column, Row);
        public DataInventory DataInventory { get; private set; }
        public UnityEvent DataChangeEvent { get; private set; } = new UnityEvent();

        //public int Row => row;
        [SerializeField] public int Row;

        //public int Column => column;
        [SerializeField] public int Column;

        void IDataInventorySetter.SetDataInventory(DataInventory dataInventory) => DataInventory = dataInventory;

        public override string ToString()
        {
            return DataInventory + Vector2Int.ToString();
        }

        object ICloneable.Clone() => Clone();

        public abstract DataSlot Clone();

        protected DataSlot CloneDataSlot()
        {
            var clone = (DataSlot)MemberwiseClone();
            clone.DataChangeEvent = new UnityEvent();
            return clone;
        }
    }
}
