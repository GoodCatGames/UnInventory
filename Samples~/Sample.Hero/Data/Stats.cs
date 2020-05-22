using System;
using UnInventory.Core.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace UnInventory.Samples.Sample_Hero.Data
{
    [Serializable]
    public class Stats : IComparable<Stats>, IComparable
    {
        [NonSerialized] public UnityEvent DataChangeEvent = new UnityEvent();

        public int Strength
        {
            get => _strength;
            set => RxSimple.SetValueInvokeChangeEvent(ref _strength, value, DataChangeEvent);
        }
        [SerializeField] private int _strength;

        public int Dextery
        {
            get => _dextery;
            set => RxSimple.SetValueInvokeChangeEvent(ref _dextery, value, DataChangeEvent);
        }
        [SerializeField] private int _dextery;

        public int Intelligence
        {
            get => _intelligence;
            set => RxSimple.SetValueInvokeChangeEvent(ref _intelligence, value, DataChangeEvent);
        }
        [SerializeField] private int _intelligence;
        
        // operators
        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats()
            {
                Strength =  a.Strength + b.Strength,
                Dextery =  a.Dextery + b.Dextery,
                Intelligence =  a.Intelligence + b.Intelligence
            };
        }

        public static Stats operator -(Stats a, Stats b)
        {
            return new Stats()
            {
                _strength = a._strength - b._strength,
                _dextery = a._dextery - b._dextery,
                _intelligence = a._intelligence - b._intelligence
            };
        }

        public int CompareTo(Stats other)
        {
            var strengthComparison = Strength.CompareTo(other.Strength);
            if (strengthComparison != 0) return strengthComparison;
            var agilityComparison = Dextery.CompareTo(other.Dextery);
            return agilityComparison != 0 ? agilityComparison : Intelligence.CompareTo(other.Intelligence);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is Stats other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Stats)}");
        }

        public override string ToString()
        {
            var strength = Strength <= 0 ? "" : $"Strength: {Strength}";
            var dextery = Dextery <= 0 ? "" : $"Dextery: {Dextery}";
            var intelligence = Intelligence <= 0 ? "" : $"Intelligence: {Intelligence}";

            return $"{strength} {dextery} {intelligence}";
        }
    }
}
