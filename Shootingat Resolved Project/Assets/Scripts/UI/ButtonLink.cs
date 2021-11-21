using UnityEngine;
using UnityEngine.UI;

namespace PabloLario.UI
{
    public enum LinkName
    {
        Website,
        Github,
        Youtube,
        Twitter
    }

    [RequireComponent(typeof(Button))]
    public class ButtonLink : MonoBehaviour
    {
        [SerializeField] private LinkName linkName;

        private Button menuButton;

        private void Start()
        {
            menuButton = GetComponent<Button>();
            menuButton.onClick.AddListener(SelectButton);
        }

        private void SelectButton()
        {
            switch (linkName)
            {
                case LinkName.Github:
                    Application.OpenURL("https://github.com/Thelario");
                    break;
                case LinkName.Twitter:
                    Application.OpenURL("https://twitter.com/lario_goro");
                    break;
                case LinkName.Website:
                    Application.OpenURL("https://thelario.github.io/");
                    break;
                case LinkName.Youtube:
                    Application.OpenURL("https://www.youtube.com/channel/UC8J7WggpsWNDGeLIn2mHoig");
                    break;
                default:
                    Debug.LogWarning("Selection of Button hasn't been processed well");
                    break;
            }
        }
    }
}
