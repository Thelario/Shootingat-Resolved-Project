using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PabloLario.DungeonGeneration;
using UnityEngine;

namespace PabloLario.Tests.EditorTests
{
    public class DungeonProceduralGenerationTest
    {
        [Test]
        public void NormalRoomsCreatedIsRight()
        {
            DungeonProceduralGeneration generation = new DungeonProceduralGeneration();
            for (int i = 0; i < 100; i++)
            {
                List<RoomPos> generatedRooms = generation.CalculateValidRoomsPos(10);
                Assert.GreaterOrEqual(generatedRooms.Count, 10);
            }
        }


        [Test]
        public void AdjacentRoomsAreAlwaysCreated()
        {
            DungeonProceduralGeneration generation = new DungeonProceduralGeneration();
            for (int i = 0; i < 100; i++)
            {
                List<RoomPos> generatedRooms = generation.CalculateValidRoomsPos(6);

                Assert.True(generatedRooms.Any(room => room.Pos == Vector2Int.zero));
                Assert.True(generatedRooms.Any(room => room.Pos == Vector2Int.left));
                Assert.True(generatedRooms.Any(room => room.Pos == Vector2Int.down));
                Assert.True(generatedRooms.Any(room => room.Pos == Vector2Int.up));
                Assert.True(generatedRooms.Any(room => room.Pos == Vector2Int.right));
            }
        }


        [Test]
        public void AllDoorsAreConnectedWithNoOverlappedRooms()
        {
            DungeonProceduralGeneration generation = new DungeonProceduralGeneration();
            for (int i = 0; i < 100; i++)
            {
                List<RoomPos> generatedRooms = generation.CalculateValidRoomsPos(10);

                foreach (RoomPos room in generatedRooms)
                {
                    List<Vector2Int> neighbourPositions = room.NeighbourPositions().ToList();
                    foreach (Vector2Int neighbourPosition in neighbourPositions)
                    {
                        Assert.True(generatedRooms.Any(roomToCompare => roomToCompare.Pos == neighbourPosition),
                            $"Room in pos {room.Pos} is not with any room on {neighbourPosition}");
                    }

                    Assert.AreEqual(1, generatedRooms.Count(generatedRoom => generatedRoom.Pos == room.Pos),
                        $"Pos {room.Pos} should not contain more than 1 room");
                }
            }
        }
        

    }
}