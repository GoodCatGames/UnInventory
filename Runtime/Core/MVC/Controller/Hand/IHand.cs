using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Core.MVC.Controller.Hand
{
    public interface IHand
    {
        UnityEvent<IReadOnlyFilterNoValidCollection> NoValidFiltersEvent { get; }
        bool IsEmpty { get; }
        bool IsAreaEntity { get; }
        DataEntity DataEntityHand { get; }
        UnityEvent StatusWasChanged { get; }
        int AmountInHand { get; }

        /// <summary>
        /// Tries to take Entity from the given position, add in Hand 1 piece
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="positionCreateHand"></param>
        bool TakeEntityOnPositionInHandTry([NotNull] DataSlot slot,  Vector2 positionCreateHand);

        /// <summary>
        /// Adds % of the source count to the hand from Entity Source while possible
        /// </summary>
        /// <param name="percent"></param>
        void AddAmountInHandSourcePercent(float percent);

        /// <summary>
        /// Adds amount to the hand from Entity Source while possible
        /// </summary>
        /// <param name="amountAdd"></param>
        void AddAmountInHand(int amountAdd);

        void PutHandInSlotTry([NotNull] DataSlot slotLeftTop);
        void UndoTakeEntityInHand();
        void RemoveEntityInHand();
        void PositionSet(Vector2 position);
        bool IsCanPutHandInSlot(DataSlot slotLeftTop);
    }
}