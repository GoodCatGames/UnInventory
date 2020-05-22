using UnInventory.Core.MVC.Model.Data;
using UnityEngine;

namespace UnInventory.Samples.Sample_Hero.Data
{
    [CreateAssetMenu(fileName = "DataEntity", menuName = "UnInventory/Entity Equipment")]
    public class DataEntityEquipment : DataEntity
    {
        public DataSlotDummy.BodyPartEnum BodyPart;

        public Stats RequiredMinStats;
        public Stats BuffStatsIfEquipment;

        public bool IsTwoHanded;

        // you can not take off
        public bool IsDamned;

        // can be placed on the HotBar
        public bool IsHotBarItem;
    }
}
