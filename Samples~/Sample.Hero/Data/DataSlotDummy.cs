using System;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample_Hero.Data
{
    [Serializable]
    public class DataSlotDummy : DataSlot 
    {
        public enum BodyPartEnum
        {
            None,
            Head,
            Body,
            Hand,
            RingSlot
        }
        public BodyPartEnum BodyPart;

        public override DataSlot Clone()
        {
            var clone = (DataSlotDummy)CloneDataSlot();
            clone.BodyPart = BodyPart;
            return clone;
        }
    }
}
