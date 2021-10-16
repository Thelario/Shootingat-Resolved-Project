using System;
using TMPro;
using UnityEngine;

namespace PabloLario.UI
{
    public class UIFormatter : MonoBehaviour
    {
        [SerializeField, Tooltip("Text to be formatted, each argument will be applied with {0}, {1},etc.")]
        private string _formatter;

        private TMP_Text _textUi;

        private void Awake()
        {
            _textUi = GetComponent<TMP_Text>();
        }

        public void UpdateText(params object[] arguments)
        {
            _textUi.text = String.Format(_formatter, arguments);
        }
    }
}