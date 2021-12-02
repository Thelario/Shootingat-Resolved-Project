using PabloLario.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PabloLario.Astar;

namespace PabloLario.DungeonGeneration
{
    public enum RoomTypeOld { NormalRoom, TreasureRoom, HiddenRoom, BossRoom }

    [RequireComponent(typeof(PathRequestManager))] [RequireComponent(typeof(Pathfinding))]
    public class Room : MonoBehaviour
    {
        public List<Transform> points;
        public RoomType type;
        public RoomTypeOld oldType;                                 

        [SerializeField] private List<Door> roomDoors;              // Doors associated to the room
        [SerializeField] private List<Transform> spawnPoints;       // Points to spawn enemies
        [SerializeField] private int enemyCount;
        [SerializeField] private bool showInMinimapFromBeginning = false;

        private List<GameObject> _enemies = new List<GameObject>();
        private bool[] _spawnPointsCreated;
        private int _enemyCounter = 0;
        private GameObject _backgroundTilemap;
        private GameObject _foregroundTilemap;
        private Transform _roomCenter;

        // Pathdinging Related References
        [HideInInspector] public PathRequestManager pathRequestManager;
        private Pathfinding _pathfinding;

        // Pathfinding Related Fields
        private bool _displayGridGizmos = false;
        private LayerMask _unwalkableMask;
        private Vector2 _gridWorldSize = new Vector2(38f, 22f);
        private float _nodeRadius = .5f;
        [HideInInspector] public GridNode grid;

        private void Awake()
        {
            Transform grid = transform.Find("Grid");

            _roomCenter = transform.Find("RoomCenter");
            _unwalkableMask = LayerMask.GetMask(new string[] { "Unwalkable", "RoomExplored", "RoomUnexplored" });

            _backgroundTilemap = grid.GetChild(0).gameObject;
            _foregroundTilemap = grid.GetChild(1).gameObject;

            _pathfinding = GetComponent<Pathfinding>();
            pathRequestManager = GetComponent<PathRequestManager>();

            pathRequestManager.InitializePathRequestManager(_pathfinding);
        }

        private void Start()
        {
            _enemyCounter = enemyCount;

            if (!showInMinimapFromBeginning)
                SetLayermasks("RoomUnexplored");

            AssignRoomToDoors();
        }

        public void SetLayermasks(string lm)
        {
            _backgroundTilemap.layer = LayerMask.NameToLayer(lm);
            _foregroundTilemap.layer = LayerMask.NameToLayer(lm);
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
            }
        }

        private void HandleNormalRoom()
        {
            grid = new GridNode(_displayGridGizmos, _unwalkableMask, _gridWorldSize, _nodeRadius, _roomCenter);
            grid.CreateGrid();

            _pathfinding.InitializePathfinding(pathRequestManager, grid);

            CloseDoors();

            // Initializing the spawnPointsCreated array (this is used for checking whether an enemy has already spawned in that spawnPoint)
            _spawnPointsCreated = new bool[spawnPoints.Count];
            for (int i = 0; i < _spawnPointsCreated.Length; i++)
                _spawnPointsCreated[i] = false;

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
                _enemies.Add(go);
                go.GetComponent<IRoomAssignable>().AssignRoom(this);
                // Here I am going to need to assign to enemies with IPathRequestManagerAssignable the path request
                // manager so that they can find a path
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
            while (_spawnPointsCreated[s] == true);

            _spawnPointsCreated[s] = true;
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
            Instantiate(ItemsManager.Instance.GetRandomItemPrefab(), _roomCenter.position, Quaternion.identity);
        }

        private void SpawnPowerup()
        {
            Instantiate(AbilityPickupsManager.Instance.GetRandomAbilityPickupPrefab(), _roomCenter.position, Quaternion.identity);
        }

        private void AssignRoomToDoors()
        {
            foreach (Door d in roomDoors)
                d.RoomAssociatedWith = this;
        }

        public void ReduceEnemyCounter()
        {
            _enemyCounter -= 1;

            if (_enemyCounter <= 0)
            {
                OpenDoors();
                _displayGridGizmos = false;
            }
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawWireCube(_roomCenter.position, new Vector3(_gridWorldSize.x, _gridWorldSize.y, 1));
            if (grid != null && _displayGridGizmos)
            {
                foreach (Node n in grid.grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * ((_nodeRadius * 2) - .1f));

                    Gizmos.color = Color.green;
                }
            }
        }
    }
}