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
            for (int i = 0; i < 100; i++)
            {
                DungeonProceduralGeneration generation = new DungeonProceduralGeneration(10, 0, 0, 0);
                List<RoomPos> generatedRooms = generation.GenerateValidRoomsPos();
                Assert.GreaterOrEqual(generatedRooms.Count, 10);
            }
        }


        [Test]
        public void AdjacentRoomsAreAlwaysCreated()
        {
            for (int i = 0; i < 100; i++)
            {
                DungeonProceduralGeneration generation = new DungeonProceduralGeneration(6, 0, 0, 0);
                List<RoomPos> generatedRooms = generation.GenerateValidRoomsPos();

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
            Random.InitState(2);
            for (int i = 0; i < 1; i++)
            {
                DungeonProceduralGeneration generation = new DungeonProceduralGeneration(10, 2, 4, 1);
                List<RoomPos> generatedRooms = generation.GenerateValidRoomsPos();

                foreach (RoomPos room in generatedRooms)
                {
                    List<Vector2Int> neighbourPositions = room.NeighbourPositions().ToList();
                    foreach (Vector2Int neighbourPosition in neighbourPositions)
                    {
                        Assert.True(generatedRooms
                                .Any(roomToCompare =>
                                    roomToCompare.Pos == neighbourPosition
                                    && roomToCompare.RoomDoorsType
                                        .DoesMatchAdjacentConnections(
                                            RoomTypeBooleans
                                                .FromVector2IntDirection(room.Pos - roomToCompare.Pos))),
                            $"Room in pos {room.Pos} is not connected with any room on {neighbourPosition}");
                    }

                    Assert.AreEqual(1, generatedRooms.Count(generatedRoom => generatedRoom.Pos == room.Pos),
                        $"Pos {room.Pos} should not contain more than 1 room");
                }
            }
        }

        [Test]
        public void TreasurerRoomAreCreated()
        {
            for (int i = 0; i < 100; i++)
            {
                DungeonProceduralGeneration generation = new DungeonProceduralGeneration(10, 2, 6, 1);
                List<RoomPos> generatedRooms = generation.GenerateValidRoomsPos();
                int count = generatedRooms.Count(room => room.RoomType == RoomTypeOld.TreasureRoom);
                Assert.GreaterOrEqual(count, 2);
                Assert.LessOrEqual(count, 6);
            }
        }
    }
}