using PabloLario.Managers;
using UnityEngine;
using TMPro;

namespace PabloLario.UI
{
    public class RandomStringPickerPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private CanvasType type;

        private void OnEnable()
        {
            LoadRandomString();
        }

        private void LoadRandomString()
        {
            if (type == CanvasType.DeadGameMenu)
                tmpText.text = LoadRandomStringFromDeadStrings();
            else
                tmpText.text = LoadRandomStringFromWinStrings();
        }

        private string LoadRandomStringFromDeadStrings()
        {
            int r = Random.Range(0, Assets.Instance.deadGamePhrases.Count);
            return Assets.Instance.deadGamePhrases[r];
        }

        private string LoadRandomStringFromWinStrings()
        {
            int r = Random.Range(0, Assets.Instance.winGamePhrases.Count);
            return Assets.Instance.winGamePhrases[r];
        }
    }
}