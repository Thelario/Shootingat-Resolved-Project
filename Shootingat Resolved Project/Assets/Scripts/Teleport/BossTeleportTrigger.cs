using UnityEngine;

namespace PabloLario.Teleport
{
    public class BossTeleportTrigger : MonoBehaviour
    {
        [SerializeField] private int sceneIndex;
        [SerializeField] private TeleportToScene teleportToScene;
        [SerializeField] private KeyPopup keyPopup;

        private bool _playerOnTeleport;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E)) 
                return;
            
            if (!_playerOnTeleport)
                return;
                
            teleportToScene.GoToScene(sceneIndex, new Vector3(0f, -21f));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _playerOnTeleport = true;
            keyPopup.EnableKeyPopup();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _playerOnTeleport = false;
            keyPopup.DisableKeyPopup();
        }
    }
}