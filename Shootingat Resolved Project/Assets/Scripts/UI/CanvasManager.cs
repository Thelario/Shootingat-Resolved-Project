using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PabloLario.Managers;
using UnityEngine.SceneManagement;

namespace PabloLario.UI
{
    public enum CanvasType
    {
        MainMenu,
        MainMenuOptionsMenu,
        InGameOptionsMenu,
        InGameMenu,
        PauseGameMenu,
        DeadGameMenu,
        WinGameMenu,
        MainMenuControlsMenu,
        InGameControlsMenu,
        AboutMenu
    }

    /// This is the class that will control the child objects and allow us to open and close each child
    public class CanvasManager : Singleton<CanvasManager>
    {
        private List<CanvasController> _canvasControllerList;
        public CanvasController lastActiveCanvas;

        protected override void Awake()
        {
            base.Awake();

            _canvasControllerList = GetComponentsInChildren<CanvasController>(true).ToList();
            // The operation of transforming an array into a list using linq, as in the previous line of code, is a huge
            // costly operation, but as we are only going to do it once at the beginning of the game and for tiny operations
            // like this is not that horrible to do it.

            // This line iterates all the menus and deactivates them, using linq.
            _canvasControllerList.ForEach(x => x.gameObject.SetActive(false));

            SwitchCanvas(CanvasType.MainMenu, false);

            Time.timeScale = 0f;
            GameManager.InvokeDelegateOnPauseGame();
        }

        public void SwitchCanvas(CanvasType type, bool loadInitialScene)
        {
            if (lastActiveCanvas != null)
            {
                lastActiveCanvas.gameObject.SetActive(false);
            }

            CanvasController desiredCanvas = _canvasControllerList.Find(x => x.canvasType == type);
            if (desiredCanvas != null)
            {
                switch (type)
                {
                    case CanvasType.InGameMenu:
                        if (loadInitialScene)
                            SceneManager.LoadScene(0);

                        GameManager.InvokeDelegateOnUnPauseGame();
                        break;
                    case CanvasType.MainMenu:
                        if (loadInitialScene)
                            SceneManager.LoadScene(0);
                        
                        GameManager.InvokeDelegateOnPauseGame();
                        break;
                    default: // All the rest of menus will pause the game when called
                        GameManager.InvokeDelegateOnPauseGame();
                        break;
                }
                
                desiredCanvas.gameObject.SetActive(true);
                lastActiveCanvas = desiredCanvas;
            }
        }
    }
}
