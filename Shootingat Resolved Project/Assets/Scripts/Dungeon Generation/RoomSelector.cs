using System.Collections.Generic;
using UnityEngine;

public enum RoomType { L, R, U, D, LR, LU, LD, RU, RD, UD, LUD, LUR, URD, LDR, LUDR }

[System.Serializable]
public class RoomSerializable
{
    public List<GameObject> roomPrefabVariants;
    public RoomType roomType;

    public GameObject GetRandomVariant() { return roomPrefabVariants[Random.Range(0, roomPrefabVariants.Count)]; }
}

public class RoomSelector : MonoBehaviour
{
    [SerializeField] private List<RoomSerializable> roomPrefabs = new List<RoomSerializable>();
    [SerializeField] private List<RoomSerializable> treasureRoomPrefabs = new List<RoomSerializable>();
    [SerializeField] private List<RoomSerializable> bossRoomPrefabs = new List<RoomSerializable>();

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

    public GameObject GetCorrectRoom(RoomType rt)
    {
        // We assume that the parameter passed to this function will always be either L, R, U or D, meaning it to be the new position
        // of the room.

        // If rt = L, it means that the new room is going to be placed in the left side, which means that only a few rooms will be able to be placed:
        RoomType roomTypeSol = RoomType.LUDR;

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

        // Iterate through the list in order to find the correct room
        foreach (RoomSerializable rs in roomPrefabs)
        {
            if (rs.roomType == roomTypeSol)
                return rs.GetRandomVariant();
        }

        return null;
    }

    public GameObject GetRoomFromVariants(RoomType rt)
    {
        foreach (RoomSerializable rs in roomPrefabs)
        {
            if (rs.roomType == rt)
                return rs.GetRandomVariant();
        }

        return null;
    }

    public GameObject GetTreasureRoomFromVariants(RoomType rt)
    {
        foreach (RoomSerializable rs in treasureRoomPrefabs)
        {
            if (rs.roomType == rt)
                return rs.GetRandomVariant();
        }

        return null;
    }

    public GameObject GetBossRoomFromVariants(RoomType rt)
    {
        foreach (RoomSerializable rs in bossRoomPrefabs)
        {
            if (rs.roomType == rt)
                return rs.GetRandomVariant();
        }

        return null;
    }

    public GameObject GetRandomRoom() 
    { 
        return roomPrefabs[Random.Range(0, roomPrefabs.Count)].GetRandomVariant(); 
    }

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
