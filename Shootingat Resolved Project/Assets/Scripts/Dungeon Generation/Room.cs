using PabloLario.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PabloLario.Astar;

namespace PabloLario.DungeonGeneration
{
    public enum RoomTypeOld { NormalRoom, TreasureRoom, HiddenRoom, BossRoom }

    public class Room : MonoBehaviour
    {
        public List<Transform> points;
        public RoomType type;

        public RoomTypeOld oldType;               // Type of the room
        [SerializeField] private List<Door> roomDoors;              // Doors associated to the room
        [SerializeField] private List<Transform> spawnPoints;       // Points to spawn enemies
        [SerializeField] private int enemyCount;
        [SerializeField] private Transform roomCenter;
        [SerializeField] private bool showInMinimapFromBeginning = false;

        private List<GameObject> enemies = new List<GameObject>();
        private bool[] spawnPointsCreated;
        private int enemyCounter = 0;
        private GameObject backgroundTilemap;
        private GameObject foregroundTilemap;

        private void Awake()
        {
            Transform grid = transform.Find("Grid");
            backgroundTilemap = grid.GetChild(0).gameObject;
            foregroundTilemap = grid.GetChild(1).gameObject;
        }

        private void Start()
        {
            enemyCounter = enemyCount;

            if (!showInMinimapFromBeginning)
                SetLayermasks("RoomUnexplored");

            AssignRoomToDoors();
        }

        public void SetLayermasks(string lm)
        {
            backgroundTilemap.layer = LayerMask.NameToLayer(lm);
            foregroundTilemap.layer = LayerMask.NameToLayer(lm);
        }

        public void StartEncounter()
        {
            SetLayermasks("RoomExplored");

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

            StartCoroutine(Co_SpawnRoomEnemies());
        }

        private IEnumerator Co_SpawnRoomEnemies()
        {
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 pos = GetRandomSpawnPoint(); // For not allowing two enemies to spawn in the same spawpoint

                ParticlesManager.Instance.CreateParticle(ParticleType.EnemySpawn, pos, .5f);

                yield return new WaitForSeconds(.5f);

                GameObject go = Instantiate(EnemyManager.Instance.GetRandomEnemy(), pos, Quaternion.identity);
                enemies.Add(go);
                go.GetComponent<IRoomAssignable>().AssignRoom(this);
            }
        }

        private void CloseDoors()
        {
            foreach (Door d in roomDoors)
                d.CloseDoor();
        }

        private void OpenDoors()
        {
            foreach (Door d in roomDoors)
                d.OpenDoor();
        }

        private void DeactivateDoorTriggers()
        {
            foreach (Door d in roomDoors)
                d.DeactivateTrigger();
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
            if (Random.Range(0, 100) > 20)
                SpawnItem();
            else
                SpawnPowerup();
            
            OpenDoors();
            DeactivateDoorTriggers();
        }

        private void SpawnItem()
        {
            Instantiate(ItemsManager.Instance.GetRandomItemPrefab(), roomCenter.position, Quaternion.identity);
        }

        private void SpawnPowerup()
        {
            Instantiate(AbilityPickupsManager.Instance.GetRandomAbilityPickupPrefab(), roomCenter.position, Quaternion.identity);
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

        public void ReduceEnemyCounter()
        {
            enemyCounter -= 1;

            if (enemyCounter <= 0)
            {
                OpenDoors();
                // TODO: Play small victory sound
            }
        }
    }
}