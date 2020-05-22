using System;
using JetBrains.Annotations;
using UnityEngine;
using UnInventory.Core.Manager;

namespace UnInventory.Core.MVC.Model.Data
{
    [Serializable]
    public class DataInventory : ICloneable, IEquatable<DataInventory>
    {
        public enum TypeInventoryEnum
        {
            FreeSlots,
            Grid,
            GridSupportMultislotEntity
        }
        
        public string NameInstance;
        
        public string Id => _id;
        [SerializeField] private string _id;
        
        /// <summary>
        /// May be NoUniq!
        /// </summary>
        public string NameBlueprint => _nameBlueprint;
        [SerializeField] private string _nameBlueprint;

        public TypeInventoryEnum TypeInventory => _typeInventory;
        [SerializeField] private TypeInventoryEnum _typeInventory;

        public DataInventory([NotNull] string name, TypeInventoryEnum typeInventory)
        {
            _nameBlueprint = name ?? throw new ArgumentNullException(nameof(name));
            _typeInventory = typeInventory;
        }
        
        public void GenerateIdIfNecessary()
        {
            if (string.IsNullOrEmpty(_id))
            {
                GenerateId();
            }
        }

        private void GenerateId() => _id = InventoryManager.ContainerDiInternal.IdGen.TakeId(_nameBlueprint);

        public override string ToString() => $"{NameInstance} ({_id})";

        object ICloneable.Clone() => Clone();
       
        public DataInventory Clone()
        {
            var clone = (DataInventory) MemberwiseClone();
            clone.GenerateId();
            return clone;
        }

        // IEquatable
        public bool Equals(DataInventory other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_nameBlueprint, other._nameBlueprint) && _typeInventory == other._typeInventory;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataInventory) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_nameBlueprint != null ? _nameBlueprint.GetHashCode() : 0) * 397) ^ (int) _typeInventory;
            }
        }
    }
}
