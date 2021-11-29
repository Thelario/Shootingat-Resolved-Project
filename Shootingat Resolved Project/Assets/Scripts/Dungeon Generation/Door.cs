using PabloLario.Managers;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform doorTrigger;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private BoxCollider2D doorCollider;

        public Room RoomAssociatedWith { get; set; }

        public void OpenDoor()
        {
            // TODO: Animate the door opening

            sr.enabled = false;                    // Disable door sprite 
            doorCollider.enabled = false;          // Disable door collider
            
            if (RoomAssociatedWith.oldType == RoomTypeOld.NormalRoom)
                SoundManager.Instance.PlaySound(SoundType.RoomVictory, 1f);
        }

        public void CloseDoor()
        {
            //LeanTween.

            sr.enabled = true;               // Enable door sprite 
            doorCollider.enabled = true;    // Enable door collider
            DeactivateTrigger();            // Disable door trigger
            SoundManager.Instance.PlaySound(SoundType.Warning, 1f);
        }

        public void PlayerCrossedDoor()
        {
            // I don't call CloseDoor from here because I don't always need to close the doors.
            // This is why we do it from Room.
            if (RoomAssociatedWith == null)
                Debug.LogError("Room association is wrong");

            RoomAssociatedWith.StartEncounter();
        }

        public void DeactivateTrigger()
        {
            doorTrigger.gameObject.SetActive(false);
        }
    }
}
