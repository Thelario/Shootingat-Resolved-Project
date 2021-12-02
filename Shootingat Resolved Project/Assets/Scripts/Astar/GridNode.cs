using UnityEngine;
using System.Collections.Generic;

namespace PabloLario.Astar
{
	public class GridNode
	{
		private bool _displayGridGizmos;
		private LayerMask _unwalkableMask;
		private Vector2 _gridWorldSize;
		private float _nodeRadius;
		private float _nodeDiameter;
		private int _gridSizeX, _gridSizeY;

		public Node[,] grid;
		public Transform roomCenter;

		public int MaxSize { get => _gridSizeX * _gridSizeY; }

		public GridNode(bool displayGridGizmos, LayerMask unwalkableMask, Vector2 gridWorldSize, float nodeRadius, Transform roomCenter)
        {
			_displayGridGizmos = displayGridGizmos;
			_unwalkableMask = unwalkableMask;
			_gridWorldSize = gridWorldSize;
			_nodeRadius = nodeRadius;
			this.roomCenter = roomCenter;

			_nodeDiameter = _nodeRadius * 2;
			_gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
			_gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);

			CreateGrid();
        }

        public void CreateGrid()
		{
			grid = new Node[_gridSizeX, _gridSizeY];
			Vector3 worldBottomLeft = roomCenter.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.up * _gridWorldSize.y / 2;

			for (int x = 0; x < _gridSizeX; x++)
			{
				for (int y = 0; y < _gridSizeY; y++)
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius);
					bool walkable = !(Physics2D.CircleCast(worldPoint, _nodeRadius, Vector2.zero, 0f, _unwalkableMask));
					grid[x, y] = new Node(walkable, worldPoint, x, y);
				}
			}
		}

		public List<Node> GetNeighbours(Node node)
		{
			List<Node> neighbours = new List<Node>();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if ((x == 0 && y == 0))
						continue;

					int checkX = node.gridX + x;
					int checkY = node.gridY + y;

					if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
					{
						neighbours.Add(grid[checkX, checkY]);
					}
				}
			}

			return neighbours;
		}

		public Node NodeFromWorldPoint(Vector3 worldPosition)
		{
			float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
			float percentY = (worldPosition.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
			//Debug.Log("PercentX: " + percentX + ", PercentY: " + percentY);

			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);
			//Debug.Log("PercentXClamped: " + percentX + ", PercentYClamped: " + percentY);

			int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
			int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
			//Debug.Log("x: " + x + ", y: " + y);
			//Debug.Log("Grid Node (x,y) pos: " + grid[x, y].worldPosition);

			return grid[x, y];
		}

		/* OnDrawGizmos doesn't work if the class doesn't derive from monobehaviour.
		 * I am leaving this code here in case of future use.
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(_roomCenter.position, new Vector3(_gridWorldSize.x, _gridWorldSize.y, 1));
			if (grid != null && _displayGridGizmos)
			{
				foreach (Node n in grid)
				{
					Gizmos.color = (n.walkable) ? Color.white : Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter - .1f));

					Gizmos.color = Color.green;
				}
			}
		}
		*/
	}
}