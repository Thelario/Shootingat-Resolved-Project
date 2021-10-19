using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Characters.Player
{
    public class PlayerAbilityPoints : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform abilityPanelParent;

        [Header("Prefabs")]
        [SerializeField] private GameObject availableAbility;
        [SerializeField] private GameObject unavailableAbility;

        private readonly List<GameObject> abilitySlots = new List<GameObject>();

        public void UpdateAbility(int currentAbility, int currentMaxAbility)
        {
            DestroyPreviousAbility();

            for (int i = 0; i < currentAbility; i++)
            {
                GameObject go = Instantiate(availableAbility, abilityPanelParent);
                abilitySlots.Add(go);
            }

            for (int i = 0; i < currentMaxAbility - currentAbility; i++)
            {
                GameObject go = Instantiate(unavailableAbility, abilityPanelParent);
                abilitySlots.Add(go);
            }
        }

        private void DestroyPreviousAbility()
        {
            if (abilityPanelParent.childCount == 0)
                return;

            foreach (Transform t in abilityPanelParent)
            {
                Destroy(t.gameObject);
            }

            abilitySlots.Clear();
            abilitySlots.TrimExcess();
        }
    }
}