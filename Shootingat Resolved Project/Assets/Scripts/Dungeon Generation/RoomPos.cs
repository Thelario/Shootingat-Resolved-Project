using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PabloLario.DungeonGeneration;
using PabloLario.Helper;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public class RoomPos
    {
        public Vector2Int Pos { get; }

        public RoomTypeBooleans RoomDoorsType { get; }

        public RoomTypeOld RoomType { get; }

        public List<RoomPos> Neighbours { get; }

        public RoomPos(Vector2Int pos, RoomTypeBooleans roomDoorsType, RoomTypeOld roomType,
            [CanBeNull] List<RoomPos> neighbours)
        {
            Pos = pos;
            RoomDoorsType = roomDoorsType;
            RoomType = roomType;
            Neighbours = neighbours ?? new List<RoomPos>();
            Neighbours.ForEach(neighbour => neighbour.Neighbours.Add(this));
        }

        public Vector2Int? RandomNeighbourPos()
        {
            List<Vector2Int> neighbourPositions = NeighbourPositions().ToList();
            if (neighbourPositions.Count == 0)
                return null;
            return NeighbourPositions().ToList().GetRandomElement();
        }

        public IEnumerable<Vector2Int> NeighbourPositions()
        {
            if (RoomDoorsType.Up)
                yield return Pos + Vector2Int.up;
            if (RoomDoorsType.Right)
                yield return Pos + Vector2Int.right;
            if (RoomDoorsType.Down)
                yield return Pos + Vector2Int.down;
            if (RoomDoorsType.Left)
                yield return Pos + Vector2Int.left;
        }
        
        public IEnumerable<Vector2Int> NotNeighbourPositions()
        {
            if (!RoomDoorsType.Up)
                yield return Pos + Vector2Int.up;
            if (!RoomDoorsType.Right)
                yield return Pos + Vector2Int.right;
            if (!RoomDoorsType.Down)
                yield return Pos + Vector2Int.down;
            if (!RoomDoorsType.Left)
                yield return Pos + Vector2Int.left;
        }

        public IEnumerable<RoomAndNeighbourPos> MissingNeighbours()
        {
            return NeighbourPositions()
                .Where(pos =>
                    Neighbours.All(neighbour => neighbour.Pos != pos))
                .Select(pos => new RoomAndNeighbourPos(this, pos));
        }
    }
}

public class RoomAndNeighbourPos
{
    public RoomPos Room { get; }
    public Vector2Int NeighbourPos { get; }

    public RoomAndNeighbourPos(RoomPos room, Vector2Int neighbourPos)
    {
        Room = room;
        NeighbourPos = neighbourPos;
    }

    public Vector2Int DirectionTowardsRoom => Room.Pos - NeighbourPos;
    public Vector2Int DirectionTowardsNeighbour => NeighbourPos - Room.Pos;
}