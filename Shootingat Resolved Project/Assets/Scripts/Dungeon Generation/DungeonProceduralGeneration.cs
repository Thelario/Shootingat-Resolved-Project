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

            List<RoomPos> validRooms = new List<RoomPos>();

            RoomPos initialRoom = new RoomPos(Vector2Int.zero, RoomType.LUDR, RoomTypeOld.NormalRoom);

            validRooms.Add(initialRoom);

            // TODO Generate adjacent rooms to first one. This seesm to be a rule 

            for (int i = 0; i < roomsGenerated - 1; i++)
            {
                Vector2Int newPos = validRooms.GetRandomElement().RandomNeighbourPos();

                while (validRooms.Any(room => room.pos == newPos))
                {
                    newPos = validRooms.GetRandomElement().RandomNeighbourPos();
                }

                RoomTypeBooleans positionsAdjacentTaken = PositionsTaken(newPos, validRooms);

                List<RoomTypeBooleans> candidates = RoomTypeBooleans.CandidateMatches(positionsAdjacentTaken);

                RoomPos addedRoom = new RoomPos(newPos, candidates.GetRandomElement().toRoomType(), RoomTypeOld.NormalRoom);

                validRooms.Add(addedRoom);

            }

            return validRooms;
        }



        private RoomTypeBooleans PositionsTaken(Vector2Int pos, List<RoomPos> rooms)
        {

            Vector2Int leftPos = pos + Vector2Int.left;
            bool left = rooms.Any(room => room.pos == leftPos);

            Vector2Int rightPos = pos + Vector2Int.right;
            bool right = rooms.Any(room => room.pos == rightPos);

            Vector2Int upPos = pos + Vector2Int.up;
            bool up = rooms.Any(room => room.pos == upPos);

            Vector2Int downPos = pos + Vector2Int.down;
            bool down = rooms.Any(room => room.pos == downPos);


            return new RoomTypeBooleans(left, right, up, down);
        }


    }


    public class RoomPos
    {

        public Vector2Int pos { get; private set; }

        public RoomType roomDoorsType { get; private set; }

        public RoomTypeOld roomType { get; private set; }

        public RoomPos(Vector2Int pos, RoomType roomDoorsType, RoomTypeOld roomType)
        {
            this.pos = pos;
            this.roomDoorsType = roomDoorsType;
            this.roomType = roomType;
        }

        public Vector2Int RandomNeighbourPos()
        {
            return NeighbourPositions().ToList().GetRandomElement();
        }

        public IEnumerable<Vector2Int> NeighbourPositions()
        {
            yield return pos + Vector2Int.up;
            yield return pos + Vector2Int.right;
            yield return pos + Vector2Int.down;
            yield return pos + Vector2Int.left;
        }
    }
}
