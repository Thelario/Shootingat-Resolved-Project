using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Characters.Player
{
    public class PlayerAbilityPoints : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _abilityPanelParent;

        [Header("Prefabs")]
        [SerializeField] private GameObject _availableAbility;
        [SerializeField] private GameObject _unavailableAbility;

        [Header("Ability Animation")]
        [SerializeField] private float _timeBetweenItemAnimation;
        [SerializeField] private float _animationTime;
        [SerializeField] private Vector3 _defaultScale;
        [SerializeField] private Vector3 _animationScale;

        private readonly List<GameObject> abilitySlots = new List<GameObject>();

        public void UpdateAbility(int currentAbility, int currentMaxAbility)
        {
            if (_abilityPanelParent == null)
                return;

            DestroyPreviousAbility();

            for (int i = 0; i < currentAbility; i++)
            {
                GameObject go = Instantiate(_availableAbility, _abilityPanelParent);
                abilitySlots.Add(go);
            }

            for (int i = 0; i < currentMaxAbility - currentAbility; i++)
            {
                GameObject go = Instantiate(_unavailableAbility, _abilityPanelParent);
                abilitySlots.Add(go);
            }

            StartCoroutine(nameof(AnimateAbility));
        }

        private void DestroyPreviousAbility()
        {
            if (_abilityPanelParent.childCount == 0)
                return;

            foreach (Transform t in _abilityPanelParent)
            {
                Destroy(t.gameObject);
            }

            abilitySlots.Clear();
            abilitySlots.TrimExcess();
        }

        private IEnumerator AnimateAbility()
        {
            for (int i = 0; i < abilitySlots.Count; i++)
            {
                AnimateSingleAbilityItem(abilitySlots[i]);

                yield return new WaitForSeconds(_timeBetweenItemAnimation);
            }

            yield return null;
        }

        private void AnimateSingleAbilityItem(GameObject obj)
        {
            LeanTween.scale(obj, _animationScale, _animationTime);
            LeanTween.scale(obj, _defaultScale, _animationTime).setDelay(_animationTime);
        }
    }
}