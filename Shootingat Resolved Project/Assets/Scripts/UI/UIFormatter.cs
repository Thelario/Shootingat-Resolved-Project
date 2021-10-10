using System;
using TMPro;
using UnityEngine;

namespace PabloLario.UI
{
    public class UIFormatter : MonoBehaviour
    {

        [SerializeField,
         Tooltip("Text to be formatted, each argument will be applied with {0}, {1},etc.")]
        private string formatter;

        private TMP_Text textUi;

        private void Start()
        {
            textUi = GetComponent<TMP_Text>();

        }


        public void UpdateText(params object[] arguments)
        {
            textUi.text = String.Format(formatter, arguments);
        }


    }
}


