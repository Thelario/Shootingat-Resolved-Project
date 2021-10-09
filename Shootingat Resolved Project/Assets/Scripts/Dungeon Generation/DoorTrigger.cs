using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private Door doorAssociatedWith;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // A player has entered a new room not visited before
            if (collision.CompareTag("Player"))
            {
                doorAssociatedWith.PlayerCrossedDoor();
            }
        }
    }
}
