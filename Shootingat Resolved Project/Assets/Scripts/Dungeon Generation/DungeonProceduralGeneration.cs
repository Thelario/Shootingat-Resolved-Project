using System.Collections.Generic;
using System.Linq;
using PabloLario.Helper;
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

        public DungeonProceduralGeneration(int roomsToGenerate, int minTreasureRooms, int maxTreasureRooms,
            int boosRooms)
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


            AddAdjacentRooms(roomsMaze, initialRoom);

            AddRemainingNormalRooms(roomsMaze);

            int goalAmountEndRooms = Random.Range(minTreasureRooms, maxTreasureRooms + 1) + boosRooms;
            int amountEndRooms = 0;
            amountEndRooms = FixNotConnectedRooms(roomsMaze, amountEndRooms, goalAmountEndRooms);

            AddTreasureRoomsUntilMeetingGoal(amountEndRooms, goalAmountEndRooms, roomsMaze);

            ConvertFurthestRoomsFromCenterIntoBossRoom(roomsMaze);

            return roomsMaze.Rooms;
        }

        private void AddAdjacentRooms(RoomsMaze roomMaze, RoomPos initialRoom)
        {
            IEnumerable<Vector2Int> adyacentPositions = initialRoom.NeighbourPositions();
            foreach (Vector2Int adyacentPosition in adyacentPositions)
            {
                roomMaze.AddNormalRoomWithRandomDoors(adyacentPosition);
            }
        }

        private void AddRemainingNormalRooms(RoomsMaze roomMaze)
        {
            while (roomMaze.Rooms.Count < roomsToGenerate)
            {
                roomMaze.AddNormalRoomWithRandomAtRandomPosition();
            }
        }

        private static int FixNotConnectedRooms(RoomsMaze roomsMaze, int amountEndRooms, int goalAmountEndRooms)
        {
            List<RoomAndNeighbourPos> missingNeighbours = new List<RoomAndNeighbourPos>(roomsMaze.MissingNeighbours);
            missingNeighbours.Shuffle();
            foreach (RoomAndNeighbourPos notCreatedNeighbour in missingNeighbours)
            {
                Vector2Int newPos = notCreatedNeighbour.NeighbourPos;

                if (roomsMaze.IsPosInMaze(newPos) || amountEndRooms >= goalAmountEndRooms)
                {
                    RemoveDoor(roomsMaze, notCreatedNeighbour);
                }
                else
                {
                    AddTreasureRoom(roomsMaze, notCreatedNeighbour);
                    amountEndRooms++;
                }
            }

            return amountEndRooms;
        }

        private static void RemoveDoor(RoomsMaze roomsMaze, RoomAndNeighbourPos notCreatedNeighbour)
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

            roomsMaze.MissingNeighbours.Remove(notCreatedNeighbour);
        }

        private static void AddTreasureRoom(RoomsMaze roomsMaze, RoomAndNeighbourPos notCreatedNeighbour)
        {
            RoomTypeBooleans roomType =
                RoomTypeBooleans.FromVector2IntDirection(notCreatedNeighbour.DirectionTowardsRoom);


            roomsMaze.AddRoom(notCreatedNeighbour.NeighbourPos, roomType, RoomTypeOld.TreasureRoom);
        }

        private static void AddTreasureRoomsUntilMeetingGoal(int amountEndRooms, int goalAmountEndRooms,
            RoomsMaze roomsMaze)
        {
            while (amountEndRooms < goalAmountEndRooms)
            {
                RoomAndNeighbourPos neighbour = roomsMaze.AddMissingRoom();
                AddTreasureRoom(roomsMaze, neighbour);
                amountEndRooms++;
            }
        }

        private void ConvertFurthestRoomsFromCenterIntoBossRoom(RoomsMaze roomsMaze)
        {
            for (int i = 0; i < boosRooms; i++)
            {
                int furthestTreasureRoomDistance = roomsMaze.Rooms
                    .Where(room => room.RoomType == RoomTypeOld.TreasureRoom)
                    .Max(room => room.Pos.sqrMagnitude);
                RoomPos furthestTreasureRoom = roomsMaze.Rooms.Where(room => room.RoomType == RoomTypeOld.TreasureRoom)
                    .First(room => room.Pos.sqrMagnitude == furthestTreasureRoomDistance);
                furthestTreasureRoom.RoomType = RoomTypeOld.BossRoom;
                
            }
        }
    }
}