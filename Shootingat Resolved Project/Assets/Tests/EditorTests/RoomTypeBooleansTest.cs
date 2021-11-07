using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PabloLario.DungeonGeneration;

namespace PabloLario.Tests.EditorTests
{
    public class RoomTypeBooleansTest
    {
        [Test]
        public void CandidateMatchesReturnsCandidates()
        {
            for (int left = 0; left < 2; left++)
            {
                for (int up = 0; up < 2; up++)
                {
                    for (int right = 0; right < 2; right++)
                    {
                        for (int down = 0; down < 2; down++)
                        {
                            RoomTypeBooleans otherPositions =
                                new RoomTypeBooleans(left == 1, up == 1, right == 1, down == 1);
                            int openedDoors = otherPositions.OpenedDoors();
                            List<RoomTypeBooleans> candidates = RoomTypeBooleans.CandidateMatches(otherPositions);
                            int expected = openedDoors switch
                            {
                                0 => 15,
                                1 => 8,
                                2 => 4,
                                3 => 2,
                                4 => 1,
                                _ => -1
                            };
                            Assert.AreEqual(expected, candidates.Distinct().Count());
                            foreach (RoomTypeBooleans candidate in candidates)
                            {
                                int candidateOpenedDoors = candidate.OpenedDoors();
                                candidate.JoinDoors(otherPositions);
                                int newOpenedDoors = candidate.OpenedDoors();
                                Assert.AreEqual(candidateOpenedDoors, newOpenedDoors);
                            }
                        }
                    }
                }
            }
        }
    }
}