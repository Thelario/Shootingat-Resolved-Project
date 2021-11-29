using UnityEngine;
using UnityEngine.SceneManagement;

namespace PabloLario.UI
{
    public class BossHealthSlider : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
        }
    
        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex.Equals(0))
                gameObject.SetActive(false);
            else if (scene.buildIndex.Equals(1))
                gameObject.SetActive(true);
        }
    }
}
