using UnityEngine;

namespace UnInventory.Standard.MVC.Controller.Hand
{
    internal interface IHandView
    {
        void PositionSet(Vector2 position);
        void Destroy();
    }
}