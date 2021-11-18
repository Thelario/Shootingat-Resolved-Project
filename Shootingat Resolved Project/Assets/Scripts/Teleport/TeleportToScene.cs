using UnityEngine;
using UnityEngine.SceneManagement;
using PabloLario.UI;

namespace PabloLario.Teleport
{
    public class TeleportToScene : MonoBehaviour
    {
        public void GoToScene(int sceneIndex)
        {
            if (sceneIndex == 1) // Tutorial Scene
            {
                CanvasManager.Instance.SwitchCanvas(CanvasType.InGameMenu);
            }
            
            SceneManager.LoadScene(sceneIndex);
            Managers.Assets.Instance.playerTransform.position = new Vector3(0f, -2f);
        }
    }
}
