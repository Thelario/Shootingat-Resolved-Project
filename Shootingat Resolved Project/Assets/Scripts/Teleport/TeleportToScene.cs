using UnityEngine;
using UnityEngine.SceneManagement;
using PabloLario.UI;

namespace PabloLario.Teleport
{
    public class TeleportToScene : MonoBehaviour
    {
        [SerializeField] private bool reloadInitialScene;
        
        public void GoToScene(int sceneIndex, Vector3 playerStartingPosInNewScene)
        {
            SceneManager.LoadScene(sceneIndex);
            Managers.Assets.Instance.playerTransform.position = playerStartingPosInNewScene;  //new Vector3(0f, -2f);
            
            if (sceneIndex == 1) // Tutorial Scene
            {
                CanvasManager.Instance.SwitchCanvas(CanvasType.InGameMenu, reloadInitialScene);
            }
        }
    }
}
