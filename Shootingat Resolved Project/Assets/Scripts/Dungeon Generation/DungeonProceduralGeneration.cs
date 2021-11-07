using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    [System.Serializable]
    public class DungeonProceduralGeneration
    {
        public int roomsToGenerate;
        public int minTreasureRooms;
        public int maxTreasureRooms;
        public int boosRooms;

        public DungeonProceduralGeneration(int roomsToGenerate, int minTreasureRooms, int maxTreasureRooms, int boosRooms)
        {
            this.roomsToGenerate = roomsToGenerate;
            this.minTreasureRooms = minTreasureRooms;
            this.maxTreasureRooms = maxTreasureRooms;
            this.boosRooms = boosRooms;
        }

        public List<RoomPos> GenerateValidRoomsPos()
        {
            RoomsMaze roomsMaze = new RoomsMaze();

            RoomPos initialRoom = roomsMaze.AddRoom(Vector2Int.zero, RoomTypeBooleans.FromRoomType(RoomType.LUDR),
                RoomTypeOld.NormalRoom);


            AddAdjacentRooms(initialRoom, roomsMaze);

            AddRemainingNormalRooms(roomsToGenerate, roomsMaze);

            List<RoomAndNeighbourPos> missingNeighbours = new List<RoomAndNeighbourPos>(roomsMaze.MissingNeighbours);
            foreach (RoomAndNeighbourPos notCreatedNeighbour in missingNeighbours)
            {
                Vector2Int newPos = notCreatedNeighbour.NeighbourPos;

                if (roomsMaze.IsPosInMaze(newPos))
                {
                    RemoveDoor(notCreatedNeighbour);
                }
                else
                {
                    AddTreasureRoom(notCreatedNeighbour, roomsMaze, newPos);
                }
            }

            return roomsMaze.Rooms;
        }

        private void AddAdjacentRooms(RoomPos initialRoom, RoomsMaze roomMaze)
        {
            IEnumerable<Vector2Int> adyacentPositions = initialRoom.NeighbourPositions();
            foreach (Vector2Int adyacentPosition in adyacentPositions)
            {
                roomMaze.AddNormalRoomWithRandomDoors(adyacentPosition);
            }
        }

        private void AddRemainingNormalRooms(int roomsToGenerate, RoomsMaze roomMaze)
        {
            while (roomMaze.Rooms.Count < roomsToGenerate)
            {
                roomMaze.AddNormalRoomWithRandomAtRandomPosition();
            }
        }

        private static void RemoveDoor(RoomAndNeighbourPos notCreatedNeighbour)
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

        private static void AddTreasureRoom(RoomAndNeighbourPos notCreatedNeighbour, RoomsMaze roomsMaze,
            Vector2Int newPos)
        {
            RoomTypeBooleans roomType =
                RoomTypeBooleans.FromVector2IntDirection(notCreatedNeighbour.DirectionTowardsRoom);


            roomsMaze.AddRoom(newPos, roomType, RoomTypeOld.TreasureRoom);
        }
    }
}