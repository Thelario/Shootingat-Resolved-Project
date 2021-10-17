using UnityEngine;
using System.Collections.Generic;
using System;
using PabloLario.Managers;

namespace PabloLario.Astar
{
	public class PathRequestManager : Singleton<PathRequestManager>
	{
		Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
		PathRequest currentPathRequest;

		private Pathfinding pathfinding;

		bool isProcessingPath = false;

		protected override void Awake()
		{
			base.Awake();

			pathfinding = GetComponent<Pathfinding>();
		}

		public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
		{
			PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
			pathRequestQueue.Enqueue(newRequest);
			TryProcessNext();
		}

		void TryProcessNext()
		{
			if (!isProcessingPath && pathRequestQueue.Count > 0)
			{
				currentPathRequest = pathRequestQueue.Dequeue();
				isProcessingPath = true;
				pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
			}
		}

		public void FinishedProcessingPath(Vector3[] path, bool success)
		{
			currentPathRequest.callback(path, success);
			isProcessingPath = false;
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