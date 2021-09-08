using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for saving all the possible roomTypes in the game
/// </summary>
public enum RoomTypeOld { TreasureRoom, NormalRoom, HiddenRoom, BossRoom }

public class RoomOld : MonoBehaviour
{
    [SerializeField] private RoomTypeOld type;                 // Type of the room
    [SerializeField] private List<Door> roomDoors;          // Doors associated to the room
    [SerializeField] private List<Transform> spawnPoints;   // Points to spawn enemies
    [SerializeField] private List<GameObject> enemies;      // Enemy prefabs to be instantiated

    private bool[] spawnPointsCreated;

    private void Awake()
    {
        AssignRoomToDoors();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (Door d in roomDoors)
                d.OpenDoor();
        }
    }

    /// <summary>
    /// Method called when a player enters a room that hasn't entered yet
    /// </summary>
    public void StartEncounter()
    {
        switch(type)
        {
            case RoomTypeOld.NormalRoom:
                HandleNormalRoom();
                break;
            case RoomTypeOld.TreasureRoom:
                HandleTreasureRoom();
                break;
            case RoomTypeOld.BossRoom:
                HandleBossRoom();
                break;
            case RoomTypeOld.HiddenRoom:
                HandleHiddenRoom();
                break;
        }
    }

    private void HandleNormalRoom()
    {
        // In the future, this function will change, because enemies need to be taken randomly
        // according to the current level of the dungeon an so on.

        // Closing all doors associated to this room
        foreach (Door d in roomDoors)
            d.CloseDoor();
        
        // Initializing the spawnPointsCreated array
        spawnPointsCreated = new bool[spawnPoints.Count];
        for (int i = 0; i < spawnPointsCreated.Length; i++)
            spawnPointsCreated[i] = false;
        
        // Spawnning enemies
        foreach (GameObject enemy in enemies)
        {
            // For not allowing two enemies to spawn in the same spawpoint
            Vector3 pos = GetRandomSpawnPoint();

            // Creating the enemies in the pos calculated position
            // TODO: now i need to add a transform parent to attach all enemies, so that everytime an enemy is killed,
            // I can check how many enemies are left, and if none, open doors again.
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemySpawn, pos, 1f);
            Instantiate(enemy, pos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Returns the next spawpoint that is available for spawnning an enemy
    /// </summary>
    /// <returns> SpawnPoint where instantiating an enemy </returns>
    private Vector3 GetRandomSpawnPoint()
    {
        int s; 
        do
        {
            s = Random.Range(0, spawnPoints.Count);
        } 
        while (spawnPointsCreated[s] == true);

        spawnPointsCreated[s] = true;
        return spawnPoints[s].position;
    }

    private void HandleTreasureRoom()
    {
        // Spawn in the center of the room a random item or whathever I decide.

        // DIFFERENT IDEAS FOR THIS:
        //  - I dunno what this is going to be, because I could create a room for managing
        //    level points, and allowing the player to decide which levels they want to upgrade.
        //  - I could also try to make lots of items with different stats each (like Isaac).
        //  - I could also give points to player that can only be spent in certain stats.
    }

    private void HandleBossRoom()
    {
        // This functionality might not be here in the future.
        
        // Move the camera to where the boss is going to spawn.
        // Make an initial animation for the boss to appear.
        // Insert an image of the boss appearance and name moving from left to right.
        // Make combat animation (like a scream or smth similar).
        // And the fun begins :D
    }

    private void HandleHiddenRoom()
    {
        // I dunno what this logic can be, even if it should be here.
        // I could have several types of hidden rooms premade, and just create one of them 
        // at the beginning of the lvel generation.
    }

    /// <summary>
    /// Makes each door of this room to have a reference to this room
    /// </summary>
    private void AssignRoomToDoors()
    {
        foreach (Door d in roomDoors)
            d.RoomAssociatedWith = this;
    }
}
