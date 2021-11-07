using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PabloLario.DungeonGeneration
{
    public enum RoomType
    {
        L,
        R,
        U,
        D,
        LR,
        LU,
        LD,
        RU,
        RD,
        UD,
        LUD,
        LUR,
        URD,
        LDR,
        LUDR
    }


    public class RoomTypeBooleans
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }

        public RoomTypeBooleans(bool left, bool up, bool right, bool down)
        {
            Left = left;
            Right = right;
            Up = up;
            Down = down;
        }

        public void JoinDoors(RoomTypeBooleans other)
        {
            Left = Left || other.Left;
            Up = Up || other.Up;
            Right = Right || other.Right;
            Down = Down || other.Down;
        }

        public int OpenedDoors()
        {
            int count = 0;
            if (Up) count++;
            if (Right) count++;
            if (Down) count++;
            if (Left) count++;
            return count;
        }

        public static List<RoomTypeBooleans> CandidateMatches(bool left, bool right, bool up, bool down)
        {
            return TypesRooms.Where(room => room.MatchesRoomWithAdjacent(left, right, up, down)).ToList();
        }

        public static List<RoomTypeBooleans> CandidateMatches(RoomTypeBooleans otherPositions)
        {
            return CandidateMatches(otherPositions.Left, otherPositions.Right, otherPositions.Up, otherPositions.Down);
        }

        public bool MatchesRoomWithAdjacent(bool left, bool right, bool up, bool down)
        {
            return (!left || Left) && (!right || Right) && (!up || Up) && (!down || Down);
        }

        public bool MatchesRoomWithAdjacent(RoomTypeBooleans otherPositions)
        {
            return MatchesRoomWithAdjacent(otherPositions.Left, otherPositions.Right, otherPositions.Up,
                otherPositions.Down);
        }


        private static RoomTypeBooleans[] TypesRooms =>
            new[]
            {
                new RoomTypeBooleans(true, false, false, false),
                new RoomTypeBooleans(false, true, false, false),
                new RoomTypeBooleans(false, false, true, false),
                new RoomTypeBooleans(false, false, false, true),
                new RoomTypeBooleans(true, true, false, false),
                new RoomTypeBooleans(true, false, true, false),
                new RoomTypeBooleans(true, false, false, true),
                new RoomTypeBooleans(false, true, true, false),
                new RoomTypeBooleans(false, true, false, true),
                new RoomTypeBooleans(false, false, true, true),
                new RoomTypeBooleans(true, false, true, true),
                new RoomTypeBooleans(true, true, true, false),
                new RoomTypeBooleans(false, true, true, true),
                new RoomTypeBooleans(true, true, false, true),
                new RoomTypeBooleans(true, true, true, true),
            };

        public static RoomTypeBooleans FromVector2IntDirection(Vector2Int vector2Int)
        {
            if (vector2Int == Vector2Int.left)
            {
                return new RoomTypeBooleans(true, false, false, false);
            }

            if (vector2Int == Vector2Int.up)
            {
                return new RoomTypeBooleans(false, true, false, false);
            }

            if (vector2Int == Vector2Int.right)
            {
                return new RoomTypeBooleans(false, false, true, false);
            }

            if (vector2Int == Vector2Int.down)
            {
                return new RoomTypeBooleans(false, false, false, true);
            }

            return ErrorValue();
        }

        public static RoomTypeBooleans FromRoomType(RoomType roomType)
        {
            return roomType switch
            {
                RoomType.L => new RoomTypeBooleans(true, false, false, false),
                RoomType.R => new RoomTypeBooleans(false, true, false, false),
                RoomType.U => new RoomTypeBooleans(false, false, true, false),
                RoomType.D => new RoomTypeBooleans(false, false, false, true),
                RoomType.LR => new RoomTypeBooleans(true, false, true, false),
                RoomType.LU => new RoomTypeBooleans(true, true, false, false),
                RoomType.LD => new RoomTypeBooleans(true, false, false, true),
                RoomType.RU => new RoomTypeBooleans(false, true, true, false),
                RoomType.RD => new RoomTypeBooleans(false, true, false, true),
                RoomType.UD => new RoomTypeBooleans(false, false, true, true),
                RoomType.LUD => new RoomTypeBooleans(true, false, true, true),
                RoomType.LUR => new RoomTypeBooleans(true, true, true, false),
                RoomType.URD => new RoomTypeBooleans(false, true, true, true),
                RoomType.LDR => new RoomTypeBooleans(true, true, false, true),
                RoomType.LUDR => new RoomTypeBooleans(true, true, true, true),
                _ => ErrorValue()
            };
        }

        public RoomType ToRoomType()
        {
            // ReSharper disable once ConvertIfStatementToSwitchExpression
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (Left && !Right && !Up && !Down)
                return RoomType.L;
            if (!Left && Right && !Up && !Down)
                return RoomType.R;
            if (!Left && !Right && Up && !Down)
                return RoomType.U;
            if (!Left && !Right && !Up && Down)
                return RoomType.D;
            if (Left && Right && !Up && !Down)
                return RoomType.LR;
            if (Left && !Right && Up && !Down)
                return RoomType.LU;
            if (Left && !Right && !Up && Down)
                return RoomType.LD;
            if (!Left && Right && Up && !Down)
                return RoomType.RU;
            if (!Left && Right && !Up && Down)
                return RoomType.RD;
            if (!Left && !Right && Up && Down)
                return RoomType.UD;
            if (Left && !Right && Up && Down)
                return RoomType.LUD;
            if (Left && Right && Up && !Down)
                return RoomType.LUR;
            if (!Left && Right && Up && Down)
                return RoomType.URD;
            if (Left && Right && !Up && Down)
                return RoomType.LDR;
            if (Left && Right && Up && Down)
                return RoomType.LUDR;


            return RoomType.L;
        }

        private static RoomTypeBooleans ErrorValue()
        {
            return new RoomTypeBooleans(false, false, false, false);
        }

        protected bool Equals(RoomTypeBooleans other)
        {
            return Left == other.Left && Right == other.Right && Up == other.Up && Down == other.Down;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RoomTypeBooleans) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                hashCode = (hashCode * 397) ^ Up.GetHashCode();
                hashCode = (hashCode * 397) ^ Down.GetHashCode();
                return hashCode;
            }
        }
    }


    [Serializable]
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

        private RoomType[] left =
        {
            RoomType.URD,
            RoomType.RU,
            RoomType.RD,
            RoomType.LR,
            RoomType.R,
            RoomType.LUDR
        };

        private RoomType[] right =
        {
            RoomType.LUD,
            RoomType.LD,
            RoomType.LU,
            RoomType.LR,
            RoomType.L,
            RoomType.LUDR
        };

        private RoomType[] up =
        {
            RoomType.LDR,
            RoomType.UD,
            RoomType.RD,
            RoomType.LD,
            RoomType.D,
            RoomType.LUDR
        };

        private RoomType[] down =
        {
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