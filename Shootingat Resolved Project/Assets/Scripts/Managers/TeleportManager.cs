using UnityEngine;
using UnityEngine.SceneManagement;

namespace PabloLario.Managers
{
    public class TeleportManager : Singleton<TeleportManager>
    {
        private Transform _player;

        public bool PlayerOnTeleport { get; set; }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
        }

        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            _player = GameObject.Find("Player").transform;
        }

        public void TeleportPlayer(Vector2 newPos)
        {
            if (PlayerOnTeleport)
                _player.position = new Vector3(newPos.x, newPos.y, _player.position.z);
        }
    }
}
