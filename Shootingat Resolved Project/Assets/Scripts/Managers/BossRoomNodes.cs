using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Managers
{
    public class BossRoomNodes : Singleton<BossRoomNodes>
    {
        public List<Transform> nodes;

        private Transform _previousNode;

        protected override void Awake()
        {
            base.Awake();

            FindNodes();
        }

        private void FindNodes()
        {
            nodes = new List<Transform>();

            foreach (Transform n in transform)
                nodes.Add(n);
        }

        public Transform GetRandomNode()
        {
            int r = Random.Range(0, nodes.Count);
            
            if (_previousNode == null)
                return nodes[r];

            while(_previousNode == nodes[r])
                r = Random.Range(0, nodes.Count);

            _previousNode = nodes[r];
            return nodes[r];
        }

        public Transform GetClosestNodeToPlayer()
        {
            float dst = float.MaxValue;
            Transform currentNode = null;

            foreach (Transform t in nodes)
            {
                Vector3 dir = Assets.Instance.playerTransform.position - t.position;
                if (Mathf.Abs(dir.magnitude) < dst)
                {
                    dst = Mathf.Abs(dir.magnitude);
                    currentNode = t;
                }
            }

            if (currentNode == null)
                currentNode = GetRandomNode();

            return currentNode;
        }
    }
}
