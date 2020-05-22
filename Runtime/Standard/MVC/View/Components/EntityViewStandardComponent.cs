using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Standard.MVC.View.Components
{
    [UsedImplicitly]
    public class EntityViewStandardComponent : EntityViewComponent
    {
        private const string NameTextComponent = "AmountText";
        private Text _amountText;
        
        protected override void StartInHeir()
        {
            _amountText = transform.Cast<Transform>().Single(child => child.name == NameTextComponent).GetComponent<Text>();
        }
        
        protected override void UpdateView()
        {
            gameObject.SetActive(Data.Amount != 0);
            _amountText.text = Data.Amount.ToString();
            if (!Data.IsMayBeStack)
            {
                _amountText.text = "";
            }
        }
    }
}
