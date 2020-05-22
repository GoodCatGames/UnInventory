using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.View.Components.Slot;
using UnityEngine;
using UnityEngine.UI;

namespace UnInventory.Samples.Sample_Hero.Components
{
    [UsedImplicitly]
    public class SlotViewHotBarComponent : SlotViewComponent
    {
        private const string TextNumberQuickPanel = "TextNumberQuickPanel";
        
        protected override void StartInHeir()
        {
            var textColumn = transform.Cast<Transform>().First(tr => tr.name == TextNumberQuickPanel);
            textColumn.GetComponent<Text>().text = Data.Column.ToString();
            textColumn.gameObject.SetActive(true);
        }

        protected override void UpdateView()
        {
        }
    }
}
