using System.Collections.Generic;
using System.Linq;
using PabloLario.Helper;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    [System.Serializable]
    public class DungeonProceduralGeneration
    {
        public List<RoomPos> CalculateValidRoomsPos(int roomsGenerated)
        {
            List<RoomPos> rooms = new List<RoomPos>();

            RoomPos initialRoom = new RoomPos(Vector2Int.zero, RoomTypeBooleans.FromRoomType(RoomType.LUDR),
                RoomTypeOld.NormalRoom, null);

            rooms.Add(initialRoom);

            List<RoomPosAndNeighbour> notCreatedNeighbours = new List<RoomPosAndNeighbour>();

            AddAdjacentRooms(initialRoom, rooms);

            AddRemainingNormalRooms(roomsGenerated, rooms);

            
            foreach (RoomPos room in rooms)
            {
                notCreatedNeighbours.AddRange(room.MissingNeighbours());
            }


            foreach (RoomPosAndNeighbour notCreatedNeighbour in notCreatedNeighbours)
            {
                Vector2Int newPos = notCreatedNeighbour.NeighbourPos;

                if (rooms.Any(room => room.Pos == newPos))
                {
                    Vector2Int roomDirection = notCreatedNeighbour.DirectionTowardsNeighbour;
                    RoomPos room = notCreatedNeighbour.Room;

                    if (roomDirection == Vector2Int.left)
                    {
                        room.RoomDoorsType.Left = false;
                    }

                    else if (roomDirection == Vector2Int.up)
                    {
                        room.RoomDoorsType.Up = false;
                    }

                    else if (roomDirection == Vector2Int.right)
                    {
                        room.RoomDoorsType.Right = false;
                    }

                    else if (roomDirection == Vector2Int.down)
                    {
                        room.RoomDoorsType.Down = false;
                    }
                }
                else
                {
                    RoomTypeBooleans roomType =
                        RoomTypeBooleans.FromVector2IntDirection(notCreatedNeighbour.DirectionTowardsRoom);

                    List<RoomPos> neighbourRooms = GetNeighbourRooms(newPos, rooms);

                    RoomPos addedRoom = new RoomPos(newPos,
                        roomType,
                        RoomTypeOld.TreasureRoom, neighbourRooms);

                    rooms.Add(addedRoom);
                }
            }

            return rooms;
        }

        private void AddAdjacentRooms(RoomPos initialRoom, List<RoomPos> rooms)
        {
            IEnumerable<Vector2Int> adyacentPositions = initialRoom.NeighbourPositions();
            foreach (Vector2Int adyacentPosition in adyacentPositions)
            {
                AddRoomFromPos(adyacentPosition, rooms);
            }
        }

        private void AddRemainingNormalRooms(int roomsGenerated, List<RoomPos> validRooms)
        {
            for (int i = 0; i < roomsGenerated - 5; i++)
            {
                Vector2Int newPos = GetNewSpawnPosForRoom(validRooms);

                AddRoomFromPos(newPos, validRooms);
            }
        }

        private static Vector2Int GetNewSpawnPosForRoom(List<RoomPos> validRooms)
        {
            Vector2Int? newPos = validRooms.GetRandomElement().RandomNeighbourPos();

            while (newPos == null || validRooms.Any(room => room.Pos == newPos))
            {
                newPos = validRooms.GetRandomElement().RandomNeighbourPos();
            }

            return (Vector2Int) newPos;
        }

        private void AddRoomFromPos(Vector2Int newPos, List<RoomPos> rooms)
        {
            List<RoomTypeBooleans> candidates = GetValidRoomTypesForPos(newPos, rooms);
            RoomTypeBooleans roomType = candidates.GetRandomElement();

            List<RoomPos> neighbourRooms = GetNeighbourRooms(newPos, rooms);

            RoomPos addedRoom = new RoomPos(newPos,
                roomType,
                RoomTypeOld.NormalRoom, neighbourRooms);

            rooms.Add(addedRoom);
        }

        private static List<RoomPos> GetNeighbourRooms(Vector2Int roomPos, List<RoomPos> generatedRooms)
        {
            List<Vector2Int> neighbourPositions = roomPos.AllNeighbourPositions().ToList();

            List<RoomPos> neighbourRooms = generatedRooms.Where(room => neighbourPositions.Contains(room.Pos)).ToList();
            return neighbourRooms;
        }


        private List<RoomTypeBooleans> GetValidRoomTypesForPos(Vector2Int newPos, List<RoomPos> validRooms)
        {
            RoomTypeBooleans positionsAdjacentTaken = PositionsTaken(newPos, validRooms);

            List<RoomTypeBooleans> candidates = RoomTypeBooleans.CandidateMatches(positionsAdjacentTaken);
            return candidates;
        }


        private RoomTypeBooleans PositionsTaken(Vector2Int pos, List<RoomPos> rooms)
        {
            Vector2Int leftPos = pos + Vector2Int.left;
            bool left = rooms.Any(room => room.Pos == leftPos && room.RoomDoorsType.Right);

            Vector2Int rightPos = pos + Vector2Int.right;
            bool right = rooms.Any(room => room.Pos == rightPos && room.RoomDoorsType.Left);

            Vector2Int upPos = pos + Vector2Int.up;
            bool up = rooms.Any(room => room.Pos == upPos && room.RoomDoorsType.Down);

            Vector2Int downPos = pos + Vector2Int.down;
            bool down = rooms.Any(room => room.Pos == downPos && room.RoomDoorsType.Up);


            return new RoomTypeBooleans(left, right, up, down);
        }
    }
}