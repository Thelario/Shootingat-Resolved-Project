using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using PabloLario.UI;

namespace PabloLario.Characters.Player
{
    public class PlayerClarity : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _clarityPanelParent;

        [Header("Prefabs")]
        [SerializeField] private GameObject _availableClarity;
        [SerializeField] private GameObject _unavailableClarity;

        [Header("Clarity Animation")]
        [SerializeField] private float _timeBetweenItemAnimation;
        [SerializeField] private float _animationTime;
        [SerializeField] private Vector3 _defaultScale;
        [SerializeField] private Vector3 _animationScale;

        private readonly List<GameObject> claritySlots = new List<GameObject>();

        public void UpdateClarity(int currentClarity, int currentMaxClarity)
        {
            if (_clarityPanelParent == null)
                return;

            DestroyPreviousClarity();

            // Creates the available clarity items
            for (int i = 0; i < currentClarity; i++)
            {
                GameObject go = Instantiate(_availableClarity, _clarityPanelParent);
                claritySlots.Add(go);
            }

            // Creates the unavailable clarity items
            for (int i = 0; i < currentMaxClarity - currentClarity; i++)
            {
                GameObject go = Instantiate(_unavailableClarity, _clarityPanelParent);
                claritySlots.Add(go);
            }

            StartCoroutine(nameof(AnimateClarity));
        }

        private void DestroyPreviousClarity()
        {
            if (_clarityPanelParent.childCount == 0)
                return;

            foreach (Transform t in _clarityPanelParent)
            {
                Destroy(t.gameObject);
            }

            claritySlots.Clear();
            claritySlots.TrimExcess();
        }

        private IEnumerator AnimateClarity()
        {
            for (int i = 0; i < claritySlots.Count; i++)
            {
                AnimateSingleClarityItem(claritySlots[i]);

                yield return new WaitForSeconds(_timeBetweenItemAnimation);
            }

            yield return null;
        }

        private void AnimateSingleClarityItem(GameObject obj)
        {
            LeanTween.scale(obj, _animationScale, _animationTime);
            LeanTween.scale(obj, _defaultScale, _animationTime).setDelay(_animationTime);
        }
    }
}