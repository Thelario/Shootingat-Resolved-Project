using System.Collections.Generic;
using UnityEngine;

public enum RoomType { L, R, U, D, LR, LU, LD, RU, RD, UD, LUD, LUR, URD, LDR, LUDR }

[System.Serializable]
public class RoomSerializable
{
    public GameObject roomPrefab;
    public RoomType roomType;
}

public class RoomSelector : MonoBehaviour
{
    [SerializeField] private List<RoomSerializable> roomPrefabs = new List<RoomSerializable>();

    private RoomType[] left = new RoomType[] {
        RoomType.URD,
        RoomType.RU,
        RoomType.RD,
        RoomType.LR,
        RoomType.R,
        RoomType.LUDR
    };

    private RoomType[] right = new RoomType[] {
        RoomType.LUD,
        RoomType.LD,
        RoomType.LU,
        RoomType.LR,
        RoomType.L,
        RoomType.LUDR
    };

    private RoomType[] up = new RoomType[] {
        RoomType.LDR,
        RoomType.UD,
        RoomType.RD,
        RoomType.LD,
        RoomType.D,
        RoomType.LUDR
    };

    private RoomType[] down = new RoomType[] {
        RoomType.LUR,
        RoomType.RU,
        RoomType.UD,
        RoomType.LU,
        RoomType.U,
        RoomType.LUDR
    };

    /// <summary>
    /// Function that returns a valid room according to the room next to it
    /// </summary>
    /// <param name="rt"> rt value can only be U, D, L or R for correct functioning </param>
    /// <returns> The prefab of the room to be created </returns>
    public GameObject GetCorrectRoom(RoomType rt)
    {
        // We assume that the parameter passed to this function will always be either L, R, U or D, meaning it to be the new position
        // of the room.

        // If rt = L, it means that the new room is going to be placed in the left side, which means that only a few rooms will be able to be placed:
        RoomType roomTypeSol = RoomType.LUDR;
        //Debug.Log(roomTypeSol);

        // We switch between the possible cases of the room
        switch (rt)
        {
            case RoomType.L: // Left side
                roomTypeSol = left[Random.Range(0, left.Length)];
                break;
            case RoomType.D: // Down side
                roomTypeSol = down[Random.Range(0, down.Length)];
                break;
            case RoomType.R: // Right side
                roomTypeSol = right[Random.Range(0, right.Length)];
                break;
            case RoomType.U: // Up side
                roomTypeSol = up[Random.Range(0, up.Length)];
                break;
            default: // Problem, shouldn't come here 
                roomTypeSol = RoomType.LUDR;
                break;
        }

        //Debug.Log(roomTypeSol);

        // Iterate through the list in order to find the correct room
        foreach (RoomSerializable rs in roomPrefabs)
        {
            if (rs.roomType == roomTypeSol)
                return rs.roomPrefab;
        }

        return null;
    }

    /// <summary>
    /// Gets a random room from the ones available
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomRoom() { return roomPrefabs[Random.Range(0, roomPrefabs.Count)].roomPrefab; }

    /// <summary>
    /// Function used to get the opposite side type room from a given one
    /// </summary>
    public RoomType GetInverseRoomType(RoomType rt)
    {
        switch (rt)
        {
            case RoomType.L: // Left side
                return RoomType.R;
            case RoomType.D: // Down side
                return RoomType.U;
            case RoomType.R: // Right side
                return RoomType.L;
            case RoomType.U: // Up side
                return RoomType.D;
            default: // Problem, shouldn't come here 
                return RoomType.R;
        }
    }
}