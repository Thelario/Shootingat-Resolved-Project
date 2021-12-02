using UnityEngine;
using System.Collections.Generic;
using System;

namespace PabloLario.Astar
{
	public class PathRequestManager : MonoBehaviour
	{
		private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
		private PathRequest _currentPathRequest;

		private Pathfinding _pathfinding;

		private bool _isProcessingPath = false;

		public void InitializePathRequestManager(Pathfinding pathfinding)
        {
			_pathfinding = pathfinding;
        }

		public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
		{
			//print("Path Start: " + pathStart.ToString() + ", Path End: " + pathEnd.ToString());

			PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
			_pathRequestQueue.Enqueue(newRequest);

			TryProcessNext();
		}

		private void TryProcessNext()
		{
			if (!_isProcessingPath && _pathRequestQueue.Count > 0)
			{
				_currentPathRequest = _pathRequestQueue.Dequeue();
				_isProcessingPath = true;
				_pathfinding.StartFindPath(_currentPathRequest.pathStart, _currentPathRequest.pathEnd);
			}
		}

		public void FinishedProcessingPath(Vector3[] path, bool success)
		{
            _currentPathRequest.callback?.Invoke(path, success);
            _isProcessingPath = false;
			TryProcessNext();
		}

		struct PathRequest
		{
			public Vector3 pathStart;
			public Vector3 pathEnd;
			public Action<Vector3[], bool> callback;

			public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
			{
				pathStart = _start;
				pathEnd = _end;
				callback = _callback;
			}
		}
	}
}