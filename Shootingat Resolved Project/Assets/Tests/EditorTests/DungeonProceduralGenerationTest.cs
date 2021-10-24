
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PabloLario.DungeonGeneration;

public class DungeonProceduralGenerationTest
{


    [Test]
    public void ValidateRoomsSpawnPoints()
    {
        DungeonProceduralGeneration generation = new DungeonProceduralGeneration();

        List<RoomPos> validRooms = generation.CalculateValidRoomsPos(10);

        Assert.GreaterOrEqual(validRooms.Count(), 10);

    }
}
