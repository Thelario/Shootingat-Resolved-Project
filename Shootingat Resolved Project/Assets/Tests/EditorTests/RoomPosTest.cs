using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PabloLario.DungeonGeneration;
using UnityEngine;

public class RoomPosTest
{
    [Test]
    public void GetNeighbourPositionsReturnsOnlyOpenedPositions()
    {
        RoomPos roomPos = new RoomPos(Vector2Int.zero,
            new RoomTypeBooleans(true, false, true, true),
            RoomTypeOld.NormalRoom, null);

        List<Vector2Int> neighbourPositions = roomPos.NeighbourPositions().ToList();

        Assert.AreEqual(3, neighbourPositions.Count);

        Assert.True(neighbourPositions.Contains(Vector2Int.left));
        Assert.True(neighbourPositions.Contains(Vector2Int.right));
        Assert.True(neighbourPositions.Contains(Vector2Int.down));
    }

    [Test]
    public void NeighbourPositionsReturnsEmptyEnumerableWithNoDoors()
    {
        RoomPos roomPos = new RoomPos(Vector2Int.zero,
            new RoomTypeBooleans(false, false, false, false),
            RoomTypeOld.NormalRoom, null);
        
        Assert.AreEqual(Enumerable.Empty<RoomPosAndNeighbour>(), roomPos.NeighbourPositions());
    }

    [Test]
    public void RoomWithOneNeighbourAndOneEmptyDoorReturnsOneEmptyNeighbour()
    {
        RoomPos roomPos = new RoomPos(Vector2Int.zero,
            new RoomTypeBooleans(false, true, true, false),
            RoomTypeOld.NormalRoom, null);

        RoomPos roomPosRight = new RoomPos(Vector2Int.right,
            new RoomTypeBooleans(true, false, false, false),
            RoomTypeOld.NormalRoom, new List<RoomPos> {roomPos});

        List<RoomPosAndNeighbour> missingNeighbours = roomPos.MissingNeighbours().ToList();

        Assert.AreEqual(1, missingNeighbours.Count);

        Assert.AreEqual(Vector2Int.up, missingNeighbours[0].NeighbourPos);
        
        Assert.AreEqual(0, roomPosRight.MissingNeighbours().Count());
    }
}