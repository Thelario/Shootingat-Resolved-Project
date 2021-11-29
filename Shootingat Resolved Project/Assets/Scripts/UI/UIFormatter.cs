using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PabloLario.UI
{
    public class UIFormatter : MonoBehaviour
    {
        [SerializeField, Tooltip("Text to be formatted, each argument will be applied with {0}, {1},etc.")]
        private string _formatter;

        [Header("Color change animation when text updates")]
        [SerializeField] private float colorChangeTime;
        [SerializeField] private Color defaultTextColor;
        [SerializeField] private Color animatedTextColor;

        [Header("Scale change animation when text updates")] 
        [SerializeField] private Vector3 defaultScale;
        [SerializeField] private Vector3 increasedScale;
        
        private TMP_Text _textUi;

        private void Awake()
        {
            _textUi = GetComponent<TMP_Text>();
        }

        public void UpdateText(params object[] arguments)
        {
            if (_textUi != null)
            {
                _textUi.text = String.Format(_formatter, arguments);
                
                if (_textUi.isActiveAndEnabled)
                    StartCoroutine(nameof(AnimateText));
            }
        }

        private IEnumerator AnimateText()
        {
            _textUi.color = animatedTextColor;
            
            LeanTween.scale(gameObject, increasedScale, colorChangeTime / 2f);

            yield return new WaitForSeconds(colorChangeTime / 2f);

            LeanTween.scale(gameObject, defaultScale, colorChangeTime / 2f);
            
            yield return new WaitForSeconds(colorChangeTime / 2f);

            _textUi.color = defaultTextColor;
        }
    }
}