using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomTypeOld { NormalRoom, TreasureRoom, HiddenRoom, BossRoom }

public class Room : MonoBehaviour
{
    public List<Transform> points;
    public RoomType type;

    [SerializeField] private RoomTypeOld oldType;               // Type of the room
    [SerializeField] private List<Door> roomDoors;              // Doors associated to the room
    [SerializeField] private List<Transform> spawnPoints;       // Points to spawn enemies
    [SerializeField] private int enemyCount;

    private List<GameObject> enemies = new List<GameObject>();
    private bool[] spawnPointsCreated;
    private int enemyCounter = 0;

    private void Start()
    {
        enemyCounter = enemyCount;

        AssignRoomToDoors();
    }

    public void StartEncounter()
    {
        switch (oldType)
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

        CloseDoors();

        // Initializing the spawnPointsCreated array
        spawnPointsCreated = new bool[spawnPoints.Count];
        for (int i = 0; i < spawnPointsCreated.Length; i++)
            spawnPointsCreated[i] = false;

        StartCoroutine(SpawnRoomEnemies());
    }

    private IEnumerator SpawnRoomEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 pos = GetRandomSpawnPoint(); // For not allowing two enemies to spawn in the same spawpoint

            yield return new WaitForSeconds(.4f);

            ParticlesManager.Instance.CreateParticle(ParticleType.EnemySpawn, pos, 1f);

            yield return new WaitForSeconds(.1f);

            // Creating the enemies in the pos calculated position
            GameObject go = Instantiate(EnemyManager.Instance.GetRandomEnemy(), pos, Quaternion.identity);
            enemies.Add(go);
        }

        AssignRoomToEnemies();
    }

    private void CloseDoors()
    {
        // Closing all doors associated to this room
        foreach (Door d in roomDoors)
            d.CloseDoor();
    }

    private void OpenDoors()
    {
        // Closing all doors associated to this room
        foreach (Door d in roomDoors)
            d.OpenDoor();
    }

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

    private void AssignRoomToDoors()
    {
        foreach (Door d in roomDoors)
            d.RoomAssociatedWith = this;
    }

    private void AssignRoomToEnemies()
    {
        foreach (GameObject e in enemies)
            e.GetComponent<Enemy>().RoomAssociatedTo = this;
    }

    public void ReduceEnemyCounter()
    {
        enemyCounter -= 1;

        if (enemyCounter <= 0)// All enemies from the room have been defeated
        {
            OpenDoors();
            // Play small victory sound
        }
    }
}
