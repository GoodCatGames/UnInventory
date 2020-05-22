using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Controller.Hand
{
    internal class HandData
    {
        public bool IsEmpty => EntitySource == null;

        public DataEntity EntitySource;

        public int AmountInHand => DataEntityHand == default ? 0 : DataEntityHand.Amount;
        public DataEntity DataEntityHand;

        public void ReturnAmountInSource()
        {
            EntitySource.Amount += AmountInHand;
            DataEntityHand.Amount = 0;
        }

        public void Clear()
        {
            EntitySource = null;
            DataEntityHand = null;
        }
    }
}
