using PabloLario.Managers;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using System.Collections;
using PabloLario.DungeonGeneration;
using PabloLario.Astar;

namespace PabloLario.Characters.Enemies
{
    public class EnemyFollow : Enemy
    {
        [Header("Enemy Stats")]
        [SerializeField] private float enemyMoveSpeed;

		protected Vector3[] path;
		protected int targetIndex;
		protected float timeBetweenMoves = 2f;
		protected float timeBetweenMovesCounter;

		protected Assets a;

        protected virtual void Awake()
        {
            a = Assets.Instance;
			_roomAssociatedTo = FindObjectOfType<Room>();
			timeBetweenMovesCounter = timeBetweenMoves;
        }

        protected override void Start()
        {
			base.Start();

			StartCoroutine(Co_Move());
        }

		/*
        private void Update()
        {
			
			if (timeBetweenMovesCounter <= 0f)
			{
				Co_Move();
				timeBetweenMovesCounter = timeBetweenMoves;
			}
			else
				timeBetweenMovesCounter -= Time.deltaTime;
			

		//Rotate();
		}
		*/

		private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out IDamageable id) && collision.collider.CompareTag("Enemy") == false)
            {
                id.TakeDamage(1);
            }
        }

        protected virtual IEnumerator Co_Move()
        {
			/* CODE BEOFRE ASTAR IMPLEMENTATION
            Vector3 dir = a.playerTransform.position - transform.position;
            transform.position = transform.position + dir.normalized * enemyMoveSpeed * Time.deltaTime;
			

			if (Time.timeSinceLevelLoad < .3f)
			{
				yield return new WaitForSeconds(.3f);
			}
			*/

			//while (_roomAssociatedTo.pathRequestManager == null)
			//yield return new WaitForSeconds(.1f);

			PathRequestManager.Instance.RequestPath(transform.position, a.playerTransform.position, OnPathFound);

			yield return new WaitForSeconds(.1f);

			yield return Co_Move();
		}

		/* CODE BEOFRE ASTAR IMPLEMENTATION
        protected virtual void Rotate()
        {
            Vector3 dir = a.playerTransform.position - transform.position;
            transform.up = dir.normalized;
        }
		*/

        public override void Die()
        {
            //OnEnemyDead(clarityToGiveToPlayerWhenDied);
            _roomAssociatedTo.ReduceEnemyCounter();
            Destroy(Instantiate(ParticlesManager.Instance.GetParticles(ParticleType.EnemyDead), transform.position, transform.rotation), 0.5f);
            Instantiate(a.bloodSplash_1, transform.position, transform.rotation);
            Destroy(gameObject);
        }

		public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				//print("Path sucessfull");
				path = newPath;
				targetIndex = 0;
				StopCoroutine(nameof(Co_FollowPath));
				StartCoroutine(nameof(Co_FollowPath));
			}
			else
            {
				//print("Path unsucessfull");
            }
		}

		IEnumerator Co_FollowPath()
		{
			bool followingPath = true;
			int pathIndex = 0;

			if (path.Length > 0)
            {
				Vector3 currentWaypoint = path[0];

				while (followingPath)
				{
					if (transform.position == currentWaypoint)
					{
						pathIndex++;
						if (pathIndex >= path.Length)
						{
							yield return Co_Move();
							yield break;
						}
						currentWaypoint = path[pathIndex];
					}

					transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemyMoveSpeed * Time.deltaTime);
					//transform.up = (currentWaypoint - transform.position).normalized; // Rotate towards the next waypoint
					transform.up = (Assets.Instance.playerTransform.position - transform.position).normalized; // Rotate towards player
					yield return null;
				}
			}
		}

		public void OnDrawGizmos()
		{
			if (path != null)
			{
				for (int i = targetIndex; i < path.Length; i++)
				{
					Gizmos.color = Color.black;
					Gizmos.DrawCube(path[i], Vector3.one);

					if (i == targetIndex)
					{
						Gizmos.DrawLine(transform.position, path[i]);
					}
					else
					{
						Gizmos.DrawLine(path[i - 1], path[i]);
					}
				}
			}
		}
	}
}
