using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void CloseDoor()
    {
        // TODO: Animate door closing
        
        sr.enabled = true;                          // Enable door sprite 
        doorCollider.enabled = true;                // Enable door collider
        doorTrigger.gameObject.SetActive(false);    // Disable door trigger
    }

    public void PlayerCrossedDoor()
    {
        // I don't call CloseDoor from here because I don't always need to close the doors.
        // This is why we do it from Room.
        if (RoomAssociatedWith == null) Debug.LogError("Room association is wrong");
        RoomAssociatedWith.StartEncounter();
    }
}
