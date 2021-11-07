using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Managers
{
    public class BossRoomNodes : Singleton<BossRoomNodes>
    {
        public List<Transform> nodes;

        private Transform previousNode;

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
            
            if (previousNode == null)
                return nodes[r];

            while(previousNode == nodes[r])
                r = Random.Range(0, nodes.Count);

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
                return GetRandomNode();

            return currentNode;
        }
    }
}
