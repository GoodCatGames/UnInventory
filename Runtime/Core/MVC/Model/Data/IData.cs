using UnityEngine.Events;

namespace UnInventory.Core.MVC.Model.Data
{
    public interface IData
    {
        DataInventory DataInventory { get; }

        /// <summary>
        /// Should be Invoke if you change data
        /// </summary>
        UnityEvent DataChangeEvent { get; }
    }
}