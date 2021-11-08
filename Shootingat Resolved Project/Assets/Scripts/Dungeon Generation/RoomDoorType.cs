using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public static List<RoomTypeBooleans> ValidRoomsConnectingOn(bool left, bool up, bool right, bool down)
        {
            return TypesRooms.Where(room => room.DoesMatchAdjacentConnections(left, up, right, down)).ToList();
        }

        public static List<RoomTypeBooleans> ValidRoomsConnectingOn(RoomTypeBooleans connectedOn)
        {
            return ValidRoomsConnectingOn(connectedOn.Left, connectedOn.Up, connectedOn.Right, connectedOn.Down);
        }

        public static List<RoomTypeBooleans> ValidRoomsConnectedOnAndDisconnectedOn(RoomTypeBooleans connectedOn,
            RoomTypeBooleans disconnectedOn)
        {
            return ValidRoomsConnectingOn(connectedOn).Where(room =>
                room.DoesMatchAdjacentDisconnections(disconnectedOn.Left, disconnectedOn.Up, disconnectedOn.Right,
                    disconnectedOn.Down)).ToList();
        }

        public bool DoesMatchAdjacentConnections(bool left, bool up, bool right, bool down)
        {
            return (!left || Left) && (!up || Up) && (!right || Right) && (!down || Down);
        }

        public bool DoesMatchAdjacentConnections(RoomTypeBooleans otherPositions)
        {
            return DoesMatchAdjacentConnections(otherPositions.Left, otherPositions.Up, otherPositions.Right,
                otherPositions.Down);
        }

        public bool DoesMatchAdjacentDisconnections(bool left, bool up, bool right, bool down)
        {
            return (!left || !Left) && (!up || !Up) && (!right || !Right) && (!down || !Down);
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
                RoomType.U => new RoomTypeBooleans(false, true, false, false),
                RoomType.R => new RoomTypeBooleans(false, false, true, false),
                RoomType.D => new RoomTypeBooleans(false, false, false, true),
                RoomType.LR => new RoomTypeBooleans(true, false, true, false),
                RoomType.LU => new RoomTypeBooleans(true, true, false, false),
                RoomType.LD => new RoomTypeBooleans(true, false, false, true),
                RoomType.RU => new RoomTypeBooleans(false, true, true, false),
                RoomType.RD => new RoomTypeBooleans(false, true, false, true),
                RoomType.UD => new RoomTypeBooleans(false, false, true, true),
                RoomType.LUD => new RoomTypeBooleans(true, true, false, true),
                RoomType.LUR => new RoomTypeBooleans(true, true, true, false),
                RoomType.URD => new RoomTypeBooleans(false, true, true, true),
                RoomType.LDR => new RoomTypeBooleans(true, false, true, true),
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

}