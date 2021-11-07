using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Characters.Player
{
    public class PlayerClarity : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform clarityPanelParent;

        [Header("Prefabs")]
        [SerializeField] private GameObject availableClarity;
        [SerializeField] private GameObject unavailableClarity;

        private readonly List<GameObject> claritySlots = new List<GameObject>();

        public void UpdateClarity(int currentClarity, int currentMaxClarity)
        {
            if (clarityPanelParent == null)
                return;

            DestroyPreviousClarity();

            // Creates the available clarity items
            for (int i = 0; i < currentClarity; i++)
            {
                GameObject go = Instantiate(availableClarity, clarityPanelParent);
                claritySlots.Add(go);
            }

            // Creates the unavailable clarity items
            for (int i = 0; i < currentMaxClarity - currentClarity; i++)
            {
                GameObject go = Instantiate(unavailableClarity, clarityPanelParent);
                claritySlots.Add(go);
            }
        }

        private void DestroyPreviousClarity()
        {
            if (clarityPanelParent.childCount == 0)
                return;

            foreach (Transform t in clarityPanelParent)
            {
                Destroy(t.gameObject);
            }

            claritySlots.Clear();
            claritySlots.TrimExcess();
        }
    }
}