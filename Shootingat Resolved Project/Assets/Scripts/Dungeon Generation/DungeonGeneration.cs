using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    #region FIELDS, REFERENCES AND VARIABLES

    [SerializeField] private List<GameObject> rooms = new List<GameObject>();

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

    /// <summary>
    /// Function used everytime we want to generate a dungeon. It will be based on the level (making it bigger according to the level).
    /// </summary>
    /// <param name="dungeonLevel"> Level of the dungeon we want to generate </param>
    public void GenerateDungeon(int dungeonLevel)
    {
        // For now I will omit the logic of levels

        // Calculate how many rooms we want to spawn
        numRooms = Random.Range(minNumRooms, maxNumRooms);
        numRoomsCounter = 0;

        StartCoroutine(CreateRoom(Vector2.zero));

        // All rooms are going to be equal size (all rectangular like in the binding of isaac).
        // There are not going to be connections between rooms, all rooms will connect between them.
        // 1. We generate the initial room in the center position of the grid.
        // 2. From there, we generate a valid room in each side.
        // 3. We repeat this for each new room created until we have created all the rooms
    }

    /// <summary>
    /// Given a position, we create a random room in that position and start the dungeon generation
    /// </summary>
    /// <param name="pos"> Position to create the room </param>
    public IEnumerator CreateRoom(Vector3 pos)
    {
        // Before creating the room, I need to check whether there is already a room created in that position.
        if (!CanPlaceRoom(pos))
            yield break;

        roomCreatedPositions.Add(pos);

        // Get a room that can be connected with the current room.
        GameObject roomToBeCreated = GetRandomRoom();

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

        CheckDungeon();
    }

    /// <summary>
    /// Given a position, we create a room according to the type passed as a parameter
    /// </summary>
    /// <param name="pos"> Position to create the room </param>
    /// <param name="rt"> Type of room we want to create </param>
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

    /// <summary>
    /// We create the room but we don't keep on generating the rest of the dungeon, only this room
    /// </summary>
    /// <param name="pos"> Position to place the room </param>
    /// <param name="rt"> Type of room to create </param>
    public void CreateRoom(Vector3 pos, RoomType rt, bool b)
    {
        roomCreatedPositions.Add(pos);

        GameObject roomToBeCreated = null;
        foreach (GameObject go in rooms)
        {
            if (rt == go.GetComponent<Room>().type)
            {
                roomToBeCreated = go;
                break;
            }
        }

        GameObject g = Instantiate(roomToBeCreated, pos, Quaternion.identity, transform);
        Room r = g.GetComponent<Room>();
        numRoomsCounter++;
        auxRoomsCreated.Add(r);
    }

    /// <summary>
    /// Function called whenever we want to check wheter there is already a room created in the pos position
    /// </summary>
    /// <param name="pos"> Position to check if there is a room or not </param>
    /// <returns> True if a room can be placed, false o.w. </returns>
    private bool CanPlaceRoom(Vector2 pos)
    {
        foreach(Vector2 v2 in roomCreatedPositions)
        {
            if (pos == v2)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Gets a random room from the ones in the rooms list
    /// </summary>
    /// <returns> A random rooms </returns>
    private GameObject GetRandomRoom() 
    { 
        return rooms[Random.Range(0, rooms.Count)]; 
    }

    /// <summary>
    /// Function used to get a valid room between all the possible rooms that exist.
    /// </summary>
    /// <param name="pos"> Position where the room will be created </param>
    /// <returns></returns>
    private GameObject GetValidRoom(Vector3 pos, RoomType rt)
    {
        GameObject go;
        GameObject g;
        Room r;

        // We can only return a room as a valid room if it connects with the previous room and if for each side, we will be able to place another room.
        int i = 0;
        while (true /* i < 100*/)
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

        //return roomSelector.GetCorrectRoom(rt);
    }

    /// <summary>
    /// For every side of a room, we check if it is possible to place another room in that side or if there is already a room.
    /// </summary>
    /// <param name="r"> Room to check </param>
    /// <returns> Boolean indicating whether there is a room or not </returns>
    private bool CheckRoomPoints(Room r, RoomType rt)
    {
        foreach(Transform p in r.points)
        {
            //Debug.Log("Type of Point from room: " + p.GetComponent<Point>().type);
            //Debug.Log("Type of Point not to check: " + rt);

            if (p.GetComponent<Point>().type == rt) continue;

            if (!CanPlaceRoom(p.position))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Function called after having generated a dungeon, when we want to check if the dungeon is correctly generated
    /// </summary>
    private void CheckDungeon()
    {
        // Debug.Log("This happens after dungeon generation");
        // Debug.Log("Number of rooms created: " + numRoomsCounter);

        if (numRoomsCounter < minNumRooms)
        {
            RestartDungeonGeneration();
        }
        else
        {
            CheckRoomConnections();
        }
    }

    /// <summary>
    /// Function called when we want to discover if all the rooms are well generated and have not empty connections or wrong placed ones
    /// </summary>
    private void CheckRoomConnections()
    {
        // We make a first check
        foreach (Room r in roomsCreated)
        {
            foreach (Transform p in r.points)
            {
                if (CanPlaceRoom(p.position))
                {
                    // We have found a room that has empty connections

                    RoomType rt = roomSelector.GetInverseRoomType(p.GetComponent<Point>().type);
                    CreateRoom(p.position, rt, true);
                }
            }
        }

        // We add to the list the new rooms created
        foreach (Room r in auxRoomsCreated) { roomsCreated.Add(r); }

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
    }

    /// <summary>
    /// Method called when the generation of a dungeon has finnished but not enough rooms have been created.
    /// All the dungeon will be deleted and started again, until a correct dungeon is generated.
    /// </summary>
    private void RestartDungeonGeneration()
    {
        Debug.Log("Restarting dungeon");
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
