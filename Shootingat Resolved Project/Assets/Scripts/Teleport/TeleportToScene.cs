using UnityEngine;
using UnityEngine.SceneManagement;

namespace PabloLario.Teleport
{
    public class TeleportToScene : MonoBehaviour
    {
        public void GoToScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
