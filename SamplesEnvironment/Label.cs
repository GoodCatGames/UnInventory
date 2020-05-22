using System;
using UnInventory.Core.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
using Resources = UnityEngine.Resources;

namespace UnInventory.SamplesEnvironment
{
    public static class Label
    {
        private static readonly Lazy<GameObject> PrefabLazy = new Lazy<GameObject>(LoadPrefabLabel);
        private static object _context;

        /// <summary>
        /// Always new label
        /// </summary>
        /// <param name="text"></param>
        public static void Show(string text)
        {
            var contextNew = new string(new char[0]);
            Show(contextNew, text);
        }

        public static void Show(object context, string text)
        {
            var labelScript = Object.FindObjectOfType<LabelScript>();
            
            if (labelScript == null)
            {
                labelScript = CreateLabelInstance();
            }

            if (_context != context)
            {
                labelScript.SetText(text);
                _context = context;
            }
            else
            {
                labelScript.AddText(text);
            }

            labelScript.ShowTime = 3f;
        }

        private static LabelScript CreateLabelInstance()
        {
            var instantiate = PrefabLazy.Value.InstantiateOnRootCanvas();
            var labelScript = instantiate.GetComponent<LabelScript>();
            return labelScript;
        }

        private static GameObject LoadPrefabLabel()
        {
            return (GameObject) Resources.Load("Label");
        }
    }
}
