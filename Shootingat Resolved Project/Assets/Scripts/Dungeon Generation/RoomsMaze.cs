using System.Collections.Generic;
using System.Linq;
using PabloLario.Helper;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public class RoomsMaze
    {
        public List<RoomPos> Rooms { get; }
        public List<RoomAndNeighbourPos> MissingNeighbours { get; }

        public RoomsMaze()
        {
            Rooms = new List<RoomPos>();
            MissingNeighbours = new List<RoomAndNeighbourPos>();
        }

        public RoomPos AddRoom(Vector2Int newPos, RoomTypeBooleans roomDoorsType, RoomTypeOld roomType)
        {
            RoomPos addedRoom = AddRoomToList(newPos, roomDoorsType, roomType);

            RemoveMissingNeighboursFixedAfterAddingRoom(newPos, addedRoom);

            MissingNeighbours.AddRange(addedRoom.MissingNeighbours());

            return addedRoom;
        }

        public RoomPos AddNormalRoomWithRandomDoors(Vector2Int newPos)
        {
            List<RoomTypeBooleans> candidates = GetValidRoomTypesForPos(newPos);
            RoomTypeBooleans roomType = candidates.GetRandomElement();

            return AddRoom(newPos, roomType, RoomTypeOld.NormalRoom);
        }

        public RoomPos AddNormalRoomWithRandomAtRandomPosition()
        {
            Vector2Int newPos = GetRandomSpawnPosForRoom();

            return AddNormalRoomWithRandomDoors(newPos);
        }

        public bool IsPosInMaze(Vector2Int notNeighbourPosition)
        {
            return Rooms.Any(room => room.Pos == notNeighbourPosition);
        }

        public RoomAndNeighbourPos AddMissingRoom()
        {
            while (MissingNeighbours.Count == 0)
            {
                RoomPos room = Rooms.GetRandomElement();
                while (room.RoomType != RoomTypeOld.NormalRoom)
                {
                    room = Rooms.GetRandomElement();
                }
                
                IEnumerable<Vector2Int> notNeighbourPositions = room.NotNeighbourPositions();
                int openedDoors = room.RoomDoorsType.OpenedDoors();
                foreach (Vector2Int notNeighbourPosition in notNeighbourPositions)
                {
                    if (IsPosInMaze(notNeighbourPosition))
                        continue;
                    Vector2Int directionToNeighbour = notNeighbourPosition - room.Pos;
                    RoomTypeBooleans doorToOpen = RoomTypeBooleans.FromVector2IntDirection(directionToNeighbour);
                    room.RoomDoorsType.JoinDoors(doorToOpen);

                    if (room.RoomDoorsType.OpenedDoors() <= openedDoors) continue;
                    RoomAndNeighbourPos newNeighbour = new RoomAndNeighbourPos(room, notNeighbourPosition);
                    MissingNeighbours.Add(newNeighbour);
                    return newNeighbour;
                }
            }

            return MissingNeighbours.First();
        }

        private RoomPos AddRoomToList(Vector2Int newPos, RoomTypeBooleans roomDoorsType, RoomTypeOld roomType)
        {
            List<RoomPos> neighbourRooms = GetNeighbourRooms(newPos);

            RoomPos addedRoom = new RoomPos(newPos,
                roomDoorsType,
                roomType,
                neighbourRooms);

            Rooms.Add(addedRoom);
            return addedRoom;
        }

        private List<RoomPos> GetNeighbourRooms(Vector2Int roomPos)
        {
            List<Vector2Int> neighbourPositions = roomPos.AllNeighbourPositions().ToList();

            List<RoomPos> neighbourRooms = Rooms.Where(room => neighbourPositions.Contains(room.Pos)).ToList();
            return neighbourRooms;
        }

        private void RemoveMissingNeighboursFixedAfterAddingRoom(Vector2Int newPos, RoomPos addedRoom)
        {
            MissingNeighbours.RemoveAll(missingNeighbour => missingNeighbour.NeighbourPos == newPos &&
                                                            addedRoom.RoomDoorsType.DoesMatchAdjacentConnections(
                                                                RoomTypeBooleans.FromVector2IntDirection(
                                                                    missingNeighbour.DirectionTowardsRoom)));
        }


        private List<RoomTypeBooleans> GetValidRoomTypesForPos(Vector2Int newPos)
        {
            RoomTypeBooleans positionsAdjacentConnected = AdjacentRoomsConnected(newPos);
            RoomTypeBooleans positionsAdjacentDisconnected = AdjacentRoomsDisconnected(newPos);

            List<RoomTypeBooleans> candidates =
                RoomTypeBooleans.ValidRoomsConnectedOnAndDisconnectedOn
                    (positionsAdjacentConnected, positionsAdjacentDisconnected);
            return candidates;
        }

        private RoomTypeBooleans AdjacentRoomsConnected(Vector2Int pos)
        {
            Vector2Int leftPos = pos + Vector2Int.left;
            bool left = Rooms.Any(room => room.Pos == leftPos && room.RoomDoorsType.Right);

            Vector2Int rightPos = pos + Vector2Int.right;
            bool right = Rooms.Any(room => room.Pos == rightPos && room.RoomDoorsType.Left);

            Vector2Int upPos = pos + Vector2Int.up;
            bool up = Rooms.Any(room => room.Pos == upPos && room.RoomDoorsType.Down);

            Vector2Int downPos = pos + Vector2Int.down;
            bool down = Rooms.Any(room => room.Pos == downPos && room.RoomDoorsType.Up);


            return new RoomTypeBooleans(left, up, right, down);
        }

        private RoomTypeBooleans AdjacentRoomsDisconnected(Vector2Int pos)
        {
            Vector2Int leftPos = pos + Vector2Int.left;
            bool left = Rooms.Any(room => room.Pos == leftPos && !room.RoomDoorsType.Right);

            Vector2Int rightPos = pos + Vector2Int.right;
            bool right = Rooms.Any(room => room.Pos == rightPos && !room.RoomDoorsType.Left);

            Vector2Int upPos = pos + Vector2Int.up;
            bool up = Rooms.Any(room => room.Pos == upPos && !room.RoomDoorsType.Down);

            Vector2Int downPos = pos + Vector2Int.down;
            bool down = Rooms.Any(room => room.Pos == downPos && !room.RoomDoorsType.Up);


            return new RoomTypeBooleans(left, up, right, down);
        }

        private Vector2Int GetRandomSpawnPosForRoom()
        {
            AddMissingRoom();

            return MissingNeighbours.GetRandomElement().NeighbourPos;
        }
    }
}