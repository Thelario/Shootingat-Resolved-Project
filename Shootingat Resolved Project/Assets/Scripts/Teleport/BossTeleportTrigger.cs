using UnityEngine;

namespace PabloLario.Teleport
{
    public class BossTeleportTrigger : MonoBehaviour
    {
        [SerializeField] private int sceneIndex;
        [SerializeField] private TeleportToScene teleportToScene;

        private bool playerOnTeleport = false;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) 
                return;
            
            if (!playerOnTeleport)
                return;
                
            teleportToScene.GoToScene((sceneIndex));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            playerOnTeleport = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            playerOnTeleport = false;
        }
    }
}