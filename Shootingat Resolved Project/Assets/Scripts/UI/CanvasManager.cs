using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PabloLario.Managers;

namespace PabloLario.UI
{
    public enum CanvasType
    {
        MainMenu,
        DifficultyPanel,
        MainMenuOptionsMenu,
        InGameOptionsMenu,
        InGameMenu,
        PauseGameMenu,
        DeadGameMenu,
        VaultMenu,
        AboutMenu
    }

    /// This is the class that will control the child objects and allow us to open and close each child
    public class CanvasManager : Singleton<CanvasManager>
    {
        List<CanvasController> canvasControllerList;
        public CanvasController lastActiveCanvas;

        protected override void Awake()
        {
            base.Awake();

            canvasControllerList = GetComponentsInChildren<CanvasController>(true).ToList();
            // The operation of transforming an array into a list using linq, as in the previous line of code, is a huge
            // costly operation, but as we are only going to do it once at the beginning of the game and for tiny operations
            // like this is not that horrible to do it.

            // This line iterates all the menus and deactivates them, using linq.
            canvasControllerList.ForEach(x => x.gameObject.SetActive(false));

            SwitchCanvas(CanvasType.MainMenu);

            Time.timeScale = 0f;
            GameManager.InvokeDelegateOnPauseGame();
        }

        public void SwitchCanvas(CanvasType _type)
        {
            if (lastActiveCanvas != null)
            {
                lastActiveCanvas.gameObject.SetActive(false);
            }

            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);
            if (desiredCanvas != null)
            {
                switch (_type)
                {
                    case CanvasType.InGameMenu:
                        GameManager.InvokeDelegateOnUnPauseGame();
                        break;
                    case CanvasType.MainMenu:
                    case CanvasType.PauseGameMenu:
                    case CanvasType.AboutMenu:
                    case CanvasType.MainMenuOptionsMenu:
                    case CanvasType.InGameOptionsMenu:
                    case CanvasType.VaultMenu:
                        GameManager.InvokeDelegateOnPauseGame();
                        break;
                }
                
                desiredCanvas.gameObject.SetActive(true);
                lastActiveCanvas = desiredCanvas;
            }
            else { /* Debug.LogWarning("The desired menu canvas was not found!"); */ }
        }
    }
}
