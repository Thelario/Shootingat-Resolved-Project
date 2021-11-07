using System.Linq;
using NUnit.Framework;
using PabloLario.DungeonGeneration;
using UnityEngine;

namespace PabloLario.Tests.EditorTests
{
    public class RoomsMazeTest
    {
        [Test]
        public void AddRoomUpdatesMissingNeighbours()
        {
            RoomsMaze maze = new RoomsMaze();
            maze.AddRoom(Vector2Int.zero,
                RoomTypeBooleans.FromRoomType(RoomType.LUDR),
                RoomTypeOld.NormalRoom);

            maze.AddRoom(Vector2Int.left, new RoomTypeBooleans(false, true, true, false), RoomTypeOld.NormalRoom);

            Assert.AreEqual(4, maze.MissingNeighbours.Count);
            Assert.AreEqual(new Vector2Int(-1,1), maze.MissingNeighbours.Last().NeighbourPos);
        }
        
        [Test]
        public void AddingRandomRoomsDoNotRepeatposition()
        {
            RoomsMaze roomsMaze = new RoomsMaze();

            roomsMaze.AddRoom(Vector2Int.zero, RoomTypeBooleans.FromRoomType(RoomType.LUDR),
                RoomTypeOld.NormalRoom);
            
            Random.InitState(1);
            
            for (int i = 0; i < 100; i++)
            {
                roomsMaze.AddNormalRoomWithRandomAtRandomPosition();
            }
            
            Assert.IsFalse(roomsMaze.Rooms.Any(room => roomsMaze.Rooms.Count(generatedRoom => generatedRoom.Pos==room.Pos)==2));
        }
    }
}