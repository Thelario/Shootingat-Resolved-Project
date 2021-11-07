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

        

        CanvasManager canvasManager;
        Button menuButton;

        private void Start()
        {
            if (addButtonEvent)
            {
                menuButton = GetComponent<Button>();
                menuButton.onClick.AddListener(OnButtonClicked);
            }

            canvasManager = CanvasManager.Instance;
        }

        private void OnButtonClicked()
        {
            PlayButtonSound();

            switch (desiredCanvasType)
            {
                case CanvasType.InGameMenu:
                case CanvasType.MainMenu:
                    GameManager.InvokeDelegateOnUnPauseGame();
                    canvasManager.SwitchCanvas(desiredCanvasType);
                    break;
                case CanvasType.PauseGameMenu:
                case CanvasType.AboutMenu:
                case CanvasType.OptionsMenu:
                case CanvasType.VaultMenu:
                    GameManager.InvokeDelegateOnPauseGame();
                    canvasManager.SwitchCanvas(desiredCanvasType);
                    break;
                default:
                    canvasManager.SwitchCanvas(desiredCanvasType);
                    break;
            }
        }

        public void SelectButton()
        {
            PlayButtonSound();
            OnButtonClicked();
        }

        public void PlayButtonSound()
        {
            SoundManager.Instance.PlaySound(SoundType.Blop, 1f);
            
        }
    }
}