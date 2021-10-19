using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.UI
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private Vector3 popupScale;
        [SerializeField] private Vector3 defaultScale;
        [SerializeField] private float popupTime;

        public void AnimateScorePopup()
        {
            LeanTween.scale(gameObject, popupScale, popupTime).setOnComplete(SetDefaultScale);
        }

        public void SetDefaultScale()
        {
            LeanTween.scale(gameObject, defaultScale, popupTime);
        }
    }
}