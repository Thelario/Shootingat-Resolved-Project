using UnityEngine;
using UnityEngine.SceneManagement;
using PabloLario.Managers;

namespace PabloLario.Teleport
{
    public class TeleportToScene : MonoBehaviour
    {
        public void GoToScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
            Managers.Assets.Instance.playerTransform.position = new Vector3(0f, -21f);
        }
    }
}
