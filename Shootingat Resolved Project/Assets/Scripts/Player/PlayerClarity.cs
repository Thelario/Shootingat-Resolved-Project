using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClarity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform clarityPanelParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject availableClarity;
    [SerializeField] private GameObject unavailableClarity;

    private List<GameObject> claritySlots = new List<GameObject>();

    public void UpdateClarity(int currentClarity, int currentMaxClarity)
    {
        // ESTO AHORA MISMO ES UN CODIGO DE MIERDA, EN EL FUTURO BUSCARÉ LA MANERA DE MEJORAR LA EFICIENCIA DE ESTA MIERDA

        DestroyPreviousClaritySlots();

        for (int i = 0; i < currentClarity; i++)
        {
            GameObject go = Instantiate(availableClarity, clarityPanelParent);
            claritySlots.Add(go);
        }

        for (int i = 0; i < currentMaxClarity - currentClarity; i++)
        {
            GameObject go = Instantiate(unavailableClarity, clarityPanelParent);
            claritySlots.Add(go);
        }
    }

    private void DestroyPreviousClaritySlots()
    {
        if (claritySlots.Count == 0)
            return;

        foreach (GameObject go in claritySlots)
        {
            Destroy(go);
        }
    }
}
