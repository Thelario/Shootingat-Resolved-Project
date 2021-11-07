using UnityEngine;
using UnityEngine.SceneManagement;

namespace PabloLario
{
    public class TestClickScript : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
