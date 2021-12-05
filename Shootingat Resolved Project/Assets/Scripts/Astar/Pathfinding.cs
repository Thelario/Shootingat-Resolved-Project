using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace PabloLario.Astar
{
	public class Pathfinding : MonoBehaviour
	{
		private PathRequestManager _requestManager;
		private GridNode _grid;

		public void InitializePathfinding(PathRequestManager pathRequestManager, GridNode grid)
        {
			_requestManager = pathRequestManager;
			_grid = grid;
        }

		public void StartFindPath(Vector3 startPos, Vector3 targetPos)
		{
			StartCoroutine(FindPath(startPos, targetPos));
		}

		IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
		{
			//print("Path Start: " + startPos.ToString() + ", Path End: " + targetPos.ToString());

			Vector3[] waypoints = new Vector3[0];
			bool pathSuccess = false;

			/* IMPORTANT
			 * When I changed the behaviour of the pathfinding so that a grid is created for each room when player enters, I run into a 
			 * problem in which enemies were always finding the furthest node from the grid from the player. This was because I was using
			 * the center of the room in the calculations, but the position of the player and the enemies were given from the center of 
			 * the level. In order to fix this problem I calculated the positions of the player and enemies based on the room they are at,
			 * and this is achieved by the next two lines of code.*/
			Vector3 derivedStartPos = startPos - _grid.roomCenter.position;
			Vector3 derivedTargetPos = targetPos - _grid.roomCenter.position;
			
			Node startNode = _grid.NodeFromWorldPoint(derivedStartPos);
			Node targetNode = _grid.NodeFromWorldPoint(derivedTargetPos);

			//print("Start Node: " + startNode.worldPosition.ToString() + ", Target Node: " + targetNode.worldPosition.ToString());

			if (startNode.walkable && targetNode.walkable)
			{
				Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
				HashSet<Node> closedSet = new HashSet<Node>();
				openSet.Add(startNode);

				while (openSet.Count > 0)
				{
					Node currentNode = openSet.RemoveFirst();
					closedSet.Add(currentNode);

					if (currentNode == targetNode)
					{
						pathSuccess = true;
						break;
					}

					foreach (Node neighbour in _grid.GetNeighbours(currentNode))
					{
						if (!neighbour.walkable || closedSet.Contains(neighbour))
						{
							continue;
						}

						int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
						if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
						{
							neighbour.gCost = newMovementCostToNeighbour;
							neighbour.hCost = GetDistance(neighbour, targetNode);
							neighbour.parent = currentNode;

							if (!openSet.Contains(neighbour))
								openSet.Add(neighbour);
						}
					}
				}
			}

			yield return null;
			if (pathSuccess)
			{
				waypoints = RetracePath(startNode, targetNode);
			}

			_requestManager.FinishedProcessingPath(waypoints, pathSuccess);
		}

		Vector3[] RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			Vector3[] waypoints = DontSimplyPath(path);
			Array.Reverse(waypoints);
			return waypoints;
		}

		Vector3[] DontSimplyPath(List<Node> path)
        {
			List<Vector3> waypoints = new List<Vector3>();

            for (int i = 1; i < path.Count; i++)
            {
				waypoints.Add(path[i].worldPosition);
            }

			return waypoints.ToArray();
		}

		Vector3[] SimplifyPath(List<Node> path)
		{
			List<Vector3> waypoints = new List<Vector3>();
			Vector2 directionOld = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
				if (directionNew != directionOld)
				{
					waypoints.Add(path[i].worldPosition);
				}
				directionOld = directionNew;
			}
			return waypoints.ToArray();
		}

		int GetDistance(Node nodeA, Node nodeB)
		{
			int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
			int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}
	}
}