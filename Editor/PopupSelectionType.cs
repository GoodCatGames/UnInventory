using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnInventory.Core.Configuration;
using UnInventory.Core.Extensions;
using UnityEditor;

namespace UnInventory.Editor
{
    public class PopupSelectionType<T> where T : class 
    {
        private readonly string _labelPopup;
        private const string NoneName = "None";

        public Type Selected => _implementations.GetTypeOrNull(_namesTypes[_selectedIndexInPopup]);
        
        private int _selectedIndexInPopup;
        private readonly string[] _namesTypes;
        private readonly ReflectionClassImplementations<T> _implementations;

        public PopupSelectionType(string labelPopup, bool isMayBeOptional = false)
        {
            _labelPopup = labelPopup;
            _implementations = new ReflectionClassImplementations<T>();
            _namesTypes = _implementations.DictionaryNameType.Keys.ToArray();

            if (isMayBeOptional)
            {
                var list = _namesTypes.ToList();
                list.Insert(0, NoneName);
                _namesTypes = list.ToArray();
            }
            
            var defaultComponent = _implementations.DictionaryNameType.FirstOrDefault(pair =>
                pair.Value.GetCustomAttribute(typeof(IsDefaultInventoryCreatorAttribute)) != null);

            if (!defaultComponent.Equals(new KeyValuePair<string, Type>()))
            {
                _selectedIndexInPopup = _namesTypes.ToList().IndexOf(defaultComponent.Key);
            }
        }

        public void OnGuiPopup()
        {
            _selectedIndexInPopup = EditorGUILayout.Popup(_labelPopup, _selectedIndexInPopup, _namesTypes);
        }
    }
}
