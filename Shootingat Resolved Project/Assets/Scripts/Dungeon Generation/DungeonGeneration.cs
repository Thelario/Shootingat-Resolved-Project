using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public class DungeonGeneration : MonoBehaviour
    {
        #region DELEGATES

        public delegate void DungeonGenerationEnded();
        public static event DungeonGenerationEnded OnDungeonGenerationEnded;

        #endregion

        #region FIELDS, REFERENCES AND VARIABLES

        [SerializeField] private List<GameObject> rooms = new List<GameObject>();

        [SerializeField] private GameObject initialRoom;

        [SerializeField] private int minNumRooms;
        [SerializeField] private int maxNumRooms;

        private List<Vector2> roomCreatedPositions = new List<Vector2>();
        private List<Room> roomsCreated = new List<Room>();
        private List<Room> auxRoomsCreated = new List<Room>();

        private int numRooms;
        private int numRoomsCounter;

        [SerializeField] private RoomSelector roomSelector;

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            GenerateDungeon(1);
        }

        #endregion

        #region MY METHODS

        public void GenerateDungeon(int dungeonLevel)
        {
            // For now I will omit the logic of levels

            // Calculate how many rooms we want to spawn
            numRooms = Random.Range(minNumRooms, maxNumRooms);
            numRoomsCounter = 0;

            StartCoroutine(CreateRoom(Vector2.zero));
        }

        public IEnumerator CreateRoom(Vector3 pos)
        {
            // Before creating the room, I need to check whether there is already a room created in that position.
            if (!CanPlaceRoom(pos))
                yield break;

            roomCreatedPositions.Add(pos);

            // Creating the initial room (the same room in each game)
            GameObject g = Instantiate(initialRoom, pos, Quaternion.identity, transform);
            Room r = g.GetComponent<Room>();
            numRoomsCounter++;
            roomsCreated.Add(r);

            // Stop the function if the maxNumber of rooms have been created
            if (numRoomsCounter >= numRooms)
                yield break;

            // Create a new room for each side of the new room cerate
            foreach (Transform p in r.points)
            {
                //yield return new WaitForSeconds(.1f);
                yield return CreateRoom(p.position, p.GetComponent<Point>().type);
            }

            CheckDungeon();
        }

        public IEnumerator CreateRoom(Vector3 pos, RoomType rt)
        {
            // Before creating the room, I need to check whether there is already a room created in that position.
            if (!CanPlaceRoom(pos))
                yield break;

            // Get a room that can be connected with the current room.
            GameObject roomToBeCreated = GetValidRoom(pos, rt);

            roomCreatedPositions.Add(pos);

            // Actual creation of the rooms in the game
            GameObject g = Instantiate(roomToBeCreated, pos, Quaternion.identity, transform);
            Room r = g.GetComponent<Room>();
            numRoomsCounter++;
            roomsCreated.Add(r);

            // Stop the function if the maxNumber of rooms have been created
            if (numRoomsCounter >= numRooms)
                yield break;

            // Create a new room for each side of the new room cerate
            foreach (Transform p in r.points)
            {
                //yield return new WaitForSeconds(.1f);
                yield return CreateRoom(p.position, p.GetComponent<Point>().type);
            }
        }

        public void CreateRoomEnd(Vector3 pos, RoomType rt, bool treasureRoom, bool bossRoom)
        {
            roomCreatedPositions.Add(pos);

            GameObject roomToBeCreated;

            if (treasureRoom)
                roomToBeCreated = roomSelector.GetTreasureRoomFromVariants(rt);
            else if (bossRoom)
                roomToBeCreated = roomSelector.GetTreasureRoomFromVariants(rt); // TODO: CreateBossRoomFromVariants(rt);
            else
                roomToBeCreated = roomSelector.GetRoomFromVariants(rt);

            GameObject g = Instantiate(roomToBeCreated, pos, Quaternion.identity, transform);
            Room r = g.GetComponent<Room>();
            numRoomsCounter++;
            auxRoomsCreated.Add(r);
        }

        private bool CanPlaceRoom(Vector2 pos)
        {
            foreach (Vector2 v2 in roomCreatedPositions)
            {
                if (pos == v2)
                    return false;
            }

            return true;
        }

        private GameObject GetRandomRoom()
        {
            return rooms[Random.Range(0, rooms.Count)];
        }

        private GameObject GetValidRoom(Vector3 pos, RoomType rt)
        {
            GameObject go;
            GameObject g;
            Room r;

            // We can only return a room as a valid room if it connects with the previous room and if for each side, we will be able to place another room.
            int i = 0;
            while (true)
            {
                go = roomSelector.GetCorrectRoom(rt);
                g = Instantiate(go, pos, Quaternion.identity, transform);
                r = g.GetComponent<Room>();

                if (CheckRoomPoints(r, roomSelector.GetInverseRoomType(rt)))
                {
                    Destroy(g);
                    break;
                }

                Destroy(g);
                i++;
            }

            return go;
        }

        private bool CheckRoomPoints(Room r, RoomType rt)
        {
            foreach (Transform p in r.points)
            {
                if (p.GetComponent<Point>().type == rt) continue;

                if (!CanPlaceRoom(p.position))
                {
                    return false;
                }
            }

            return true;
        }

        private void CheckDungeon()
        {
            if (numRoomsCounter < minNumRooms)
            {
                RestartDungeonGeneration();
            }
            else
            {
                CheckRoomConnections();
            }
        }

        private void CheckRoomConnections()
        {
            bool createBossRoom = true;
            int treasureRooms = 0;

            // We create the rooms to complete the empty endings
            foreach (Room r in roomsCreated)
            {
                foreach (Transform p in r.points)
                {
                    if (CanPlaceRoom(p.position)) // We have found a room that has empty connections
                    {
                        RoomType rt = roomSelector.GetInverseRoomType(p.GetComponent<Point>().type);

                        if (createBossRoom) // If haven't created a boss room, then create it
                        {
                            createBossRoom = false;
                            CreateRoomEnd(p.position, rt, false, true);
                            continue;
                        }

                        treasureRooms++;
                        CreateRoomEnd(p.position, rt, true, false);
                    }
                }
            }

            if (createBossRoom == true || treasureRooms == 0)
            {
                RestartDungeonGeneration();
                return;
            }

            // We add to the list the new rooms created
            foreach (Room r in auxRoomsCreated)
                roomsCreated.Add(r);

            // We delete the auxiliary list
            auxRoomsCreated.Clear();
            auxRoomsCreated.TrimExcess();

            // We make a second check, this time deleting the dungeon and restarting if doesn't work
            foreach (Room r in roomsCreated)
            {
                foreach (Transform p in r.points)
                {
                    if (CanPlaceRoom(p.position)) // There is an empty space there
                    {
                        RestartDungeonGeneration();
                        return;
                    }

                    Room room = null;

                    // Looping through each created room in order to find the one placed in points position
                    foreach (Transform t in transform)
                    {
                        if (p.position == t.position)
                        {
                            room = t.GetComponent<Room>();
                            break;
                        }
                    }

                    bool foundConnection = false;

                    // Looping through the room points in order to find if there is a connection with the room we are trying to check
                    foreach (Transform point in room.points)
                    {
                        if (point.position == r.transform.position) // The room is well connected
                        {
                            foundConnection = true;
                        }
                    }

                    // Both rooms are connected, so there are not unconnected paths, which means we cna continue searching
                    if (foundConnection == true)
                        continue;

                    // Debug.Log(r.name);
                    // Debug.Log(r.transform.position);

                    // r is the room that we need to replace
                    // The way I am going to solve this is by regenerating the whole dungeon again, which is quite expensive to compute,
                    // but it works perfectly. I guess I will comeback in the future to try to solve this script.

                    RestartDungeonGeneration();
                    return;
                }
            }

            OnDungeonGenerationEnded?.Invoke();
        }

        private void RestartDungeonGeneration()
        {
            //Debug.Log("Restarting dungeon");
            numRooms = Random.Range(minNumRooms, maxNumRooms);
            numRoomsCounter = 0;

            roomCreatedPositions.Clear();
            roomCreatedPositions.TrimExcess();
            roomsCreated.Clear();
            roomsCreated.TrimExcess();

            foreach (Transform t in transform)
                Destroy(t.gameObject);

            StartCoroutine(CreateRoom(Vector2.zero));
        }

        #endregion
    }
}
