using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public enum RoomType { L, R, U, D, LR, LU, LD, RU, RD, UD, LUD, LUR, URD, LDR, LUDR }


    public class RoomTypeBooleans
    {

        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool Up { get; private set; }
        public bool Down { get; private set; }

        public RoomTypeBooleans(bool left, bool up, bool right, bool down)
        {
            this.Left = left;
            this.Right = right;
            this.Up = up;
            this.Down = down;
        }

        public static List<RoomTypeBooleans> CandidateMatches(bool left, bool right, bool up, bool down)
        {
            return TYPES_ROOMS.Where(room => room.MatchesRoomWithAdjacent(left, right, up, down)).ToList();

        }

        public static List<RoomTypeBooleans> CandidateMatches(RoomTypeBooleans otherPositions)
        {

            return CandidateMatches(otherPositions.Left, otherPositions.Right, otherPositions.Up, otherPositions.Down);

        }

        public bool MatchesRoomWithAdjacent(bool left, bool right, bool up, bool down)
        {
            return (!left || left == Left) &&
            (!right  || right == Right) &&
            (!up || up == Up) &&
            (!down || down == Down);
        }

        public bool MatchesRooms(RoomTypeBooleans otherPositions)
        {
            return MatchesRoomWithAdjacent(otherPositions.Left, otherPositions.Right, otherPositions.Up, otherPositions.Down);
        }


        private static readonly RoomTypeBooleans[] TYPES_ROOMS = new RoomTypeBooleans[]{
            new RoomTypeBooleans(true,false,false, false),
            new RoomTypeBooleans(false,true,false, false),
            new RoomTypeBooleans(false,true,true, false),
            new RoomTypeBooleans(false,false,false, true),
            new RoomTypeBooleans(true,true,false, false),
            new RoomTypeBooleans(true,false,true, false),
            new RoomTypeBooleans(true,false,false, true),
            new RoomTypeBooleans(false,true,true, false),
            new RoomTypeBooleans(false,true,false, true),
            new RoomTypeBooleans(false,false,true, true),
            new RoomTypeBooleans(true,false,true, true),
            new RoomTypeBooleans(true,true,true, false),
            new RoomTypeBooleans(false,true,true, true),
            new RoomTypeBooleans(true,true,false, true),
            new RoomTypeBooleans(true,true,true, true),
        };

        public static RoomTypeBooleans FromRoomType(RoomType roomType)
        {
            switch (roomType)
            {
                case RoomType.L:
                    return new RoomTypeBooleans(true, false, false, false);
                case RoomType.R:
                    return new RoomTypeBooleans(false, true, false, false);
                case RoomType.U:
                    return new RoomTypeBooleans(false, false, true, false);
                case RoomType.D:
                    return new RoomTypeBooleans(false, false, false, true);
                case RoomType.LR:
                    return new RoomTypeBooleans(true, true, false, false);
                case RoomType.LU:
                    return new RoomTypeBooleans(true, false, true, false);
                case RoomType.LD:
                    return new RoomTypeBooleans(true, false, false, true);
                case RoomType.RU:
                    return new RoomTypeBooleans(false, true, true, false);
                case RoomType.RD:
                    return new RoomTypeBooleans(false, true, false, true);
                case RoomType.UD:
                    return new RoomTypeBooleans(false, false, true, true);
                case RoomType.LUD:
                    return new RoomTypeBooleans(true, false, true, true);
                case RoomType.LUR:
                    return new RoomTypeBooleans(true, true, true, false);
                case RoomType.URD:
                    return new RoomTypeBooleans(false, true, true, true);
                case RoomType.LDR:
                    return new RoomTypeBooleans(true, true, false, true);
                case RoomType.LUDR:
                    return new RoomTypeBooleans(true, true, true, true);
                default:
                    return new RoomTypeBooleans(false, false, false, false);
            }
        }

        public RoomType toRoomType(){

            if(Left && !Right && !Up && !Down)
                return RoomType.L;
            else if(!Left && Right && !Up && !Down)
                return RoomType.R;
            else if(!Left && !Right && Up && !Down)
                return RoomType.U;
            else if(!Left && !Right && !Up && Down)
                return RoomType.D;
            else if(Left && Right && !Up && !Down)
                return RoomType.LR;
            else if(Left && !Right && Up && !Down)
                return RoomType.LU;
            else if(Left && !Right && !Up && Down)
                return RoomType.LD;
            else if(!Left && Right && Up && !Down)
                return RoomType.RU;
            else if(!Left && Right && !Up && Down)
                return RoomType.RD;
            else if(!Left && !Right && Up && Down)
                return RoomType.UD;
            else if(Left && !Right && Up && Down)
                return RoomType.LUD;
            else if(Left && !Right && Up && Down)
                return RoomType.LUR;
            else if(!Left && Right && Up && Down)
                return RoomType.URD;
            else if(Left && Right && !Up && Down)
                return RoomType.LDR;
            else if(Left && Right && Up && Down)
                return RoomType.LUDR;
            

            return RoomType.L;
        }



    }


    [System.Serializable]
    public class RoomSerializable
    {
        public List<GameObject> roomPrefabVariants;
        public RoomType roomType;

        public GameObject GetRandomVariant()
        {
            return roomPrefabVariants[Random.Range(0, roomPrefabVariants.Count)];
        }
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
}
