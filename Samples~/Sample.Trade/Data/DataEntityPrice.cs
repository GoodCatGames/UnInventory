using UnInventory.Core.Extensions;
using UnInventory.Core.MVC.Model.Data;
using UnityEngine;

namespace UnInventory.Samples.Sample.Trade.Data
{
    [CreateAssetMenu(fileName = "DataEntity", menuName = "UnInventory/Entity Price")]
    public class DataEntityPrice : DataEntity
    {
        public int Price {
            get => _price;
            set
            {
               var newValue = value < 0 ? 0 : value;
               RxSimple.SetValueInvokeChangeEvent(ref _price, newValue, DataChangeEvent);
            }
        }
        [SerializeField] private int _price;
    }
}
