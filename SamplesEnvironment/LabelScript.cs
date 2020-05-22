using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UnInventory.SamplesEnvironment
{
    [RequireComponent(typeof(Text))]
    public class LabelScript : MonoBehaviour
    {
        [SerializeField] public float ShowTime = 3f;
        private Text _text;

        public void AddText(string textAdd, string separator = "\n")
        {
            if (_text.text == textAdd) { return; }
            var stringSeparate =  string.IsNullOrEmpty(_text.text) ? "" : separator;
            textAdd = stringSeparate + textAdd;
            var result = _text.text + textAdd;
            SetText(result);
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        [UsedImplicitly]
        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        [UsedImplicitly]
        private void Update()
        {
            ShowTime -= Time.deltaTime;
            if (ShowTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
