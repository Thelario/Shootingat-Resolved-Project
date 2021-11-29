using UnityEngine;
using UnityEngine.UI;
using PabloLario.Managers;

namespace PabloLario.UI
{
    //[RequireComponent(typeof(Button))]
    public class CanvasSwitcher : MonoBehaviour, ISelectable
    {
        public CanvasType desiredCanvasType;
        public bool addButtonEvent = false;
        public bool reloadInitialScene;
        
        CanvasManager canvasManager;
        Button menuButton;

        private void Start()
        {
            if (addButtonEvent)
            {
                menuButton = GetComponent<Button>();
                menuButton.onClick.AddListener(SelectButton);
            }

            canvasManager = CanvasManager.Instance;
        }
        
        public void SelectButton()
        {
            PlayButtonSound();
            OnButtonClicked();
        }

        private void OnButtonClicked()
        {
            canvasManager.SwitchCanvas(desiredCanvasType, reloadInitialScene);
        }

        public void PlayButtonSound()
        {
            SoundManager.Instance.PlaySound(SoundType.Blop, 1f);
        }
    }
}