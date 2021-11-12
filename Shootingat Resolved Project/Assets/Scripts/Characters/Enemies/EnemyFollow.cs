using PabloLario.Managers;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using System.Collections;
using PabloLario.DungeonGeneration;
using PabloLario.Astar;

#pragma warning disable CS0618 // startColor in ParticleSystem is obsolete

namespace PabloLario.Characters.Enemies
{
    public class EnemyFollow : Enemy
    {
        [Header("Enemy Stats")]
        [SerializeField] private float enemyMoveSpeed;
		[SerializeField] private bool lookAtPlayer;
		
		protected Vector3[] path;
		protected int targetIndex;
		protected float timeBetweenMoves = 2f;
		protected float timeBetweenMovesCounter;

        protected virtual void Awake()
        {
			_roomAssociatedTo = FindObjectOfType<Room>();
			timeBetweenMovesCounter = timeBetweenMoves;
        }

        protected override void Start()
        {
			base.Start();

			StartCoroutine(Co_Move());
        }

		private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out IDamageable id) && collision.collider.CompareTag("Enemy") == false)
            {
                id.TakeDamage(1);
            }
        }

        protected virtual IEnumerator Co_Move()
        {
			PathRequestManager.Instance.RequestPath(transform.position, Assets.Instance.playerTransform.position, OnPathFound);

			yield return new WaitForSeconds(.1f);

			yield return Co_Move();
		}

        public override void Die()
        {
            GameManager.InvokeDelegateEnemyDead(abilityPointsToGiveToPlayerWhenDied);
            _roomAssociatedTo.ReduceEnemyCounter();

			GameObject deadParticles = Instantiate(ParticlesManager.Instance.GetParticles(ParticleType.EnemyDead), transform.position, Quaternion.identity);
			deadParticles.GetComponent<ParticleSystem>().startColor = hitAnimation.agentColor;
			Destroy(Instantiate(deadParticles, transform.position, transform.rotation), 0.5f);
            
			Instantiate(Assets.Instance.bloodSplash_1, transform.position, transform.rotation);
            Destroy(gameObject);
        }

		public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				path = newPath;
				targetIndex = 0;
				
				try
                {
					StopCoroutine(nameof(Co_FollowPath));
					StartCoroutine(nameof(Co_FollowPath));
				}
				catch(MissingReferenceException mre)
                {
					Debug.Log(mre);
                }
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

					if (!lookAtPlayer)
						transform.up = (currentWaypoint - transform.position).normalized; // Rotate towards the next waypoint
					else
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
