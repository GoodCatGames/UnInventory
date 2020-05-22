using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Components
{
    public class HeroComponent : MonoBehaviour
    {
        public Stats Stats
        {
            get => _stats;
            set { _stats = value; UpdateStats();}
        }
        private Stats _stats;

#pragma warning disable 649
        [SerializeField] private InputField _fieldStrength;
        [SerializeField] private InputField _fieldDextery;
        [SerializeField] private InputField _fieldIntelligence;
#pragma warning restore 649


        [UsedImplicitly]
        private void Awake()
        {
            Stats = new Stats();
            Stats.DataChangeEvent.AddListener(UpdateStats);

            _fieldStrength.onEndEdit.AddListener(value => Stats.Strength = int.Parse(value));
            _fieldDextery.onEndEdit.AddListener(value => Stats.Dextery = int.Parse(value));
            _fieldIntelligence.onEndEdit.AddListener(value => Stats.Intelligence = int.Parse(value));

            Stats.Strength = 5;
            Stats.Dextery = 4;
            Stats.Intelligence = 3;
        }

        [UsedImplicitly]
        private void OnDestroy() => Stats.DataChangeEvent.RemoveListener(UpdateStats);

        private void UpdateStats()
        {
            _fieldStrength.SetTextWithoutNotify(Stats.Strength.ToString());
            _fieldDextery.SetTextWithoutNotify(Stats.Dextery.ToString());
            _fieldIntelligence.SetTextWithoutNotify(Stats.Intelligence.ToString());
        }
    }
}
