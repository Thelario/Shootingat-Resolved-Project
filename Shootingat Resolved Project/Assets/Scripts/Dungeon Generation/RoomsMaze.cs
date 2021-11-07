using System.Collections.Generic;

namespace PabloLario.DungeonGeneration
{
    public class RoomsMaze
    {
        public List<RoomPos> RoomsPos { get; }
        public List<RoomPosAndNeighbour> MissingNeightbours { get; }

        public RoomsMaze()
        {
            RoomsPos = new List<RoomPos>();
            MissingNeightbours = new List<RoomPosAndNeighbour>();
        }
    }
}