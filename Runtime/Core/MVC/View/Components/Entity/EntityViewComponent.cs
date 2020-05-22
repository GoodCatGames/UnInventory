using UnityEngine;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Core.MVC.View.Components.Entity
{
    [RequireComponent(typeof(IEntityRootComponent))]
    public abstract class EntityViewComponent : ViewComponent<DataEntity, IEntityRootComponent>
    {
    }
}