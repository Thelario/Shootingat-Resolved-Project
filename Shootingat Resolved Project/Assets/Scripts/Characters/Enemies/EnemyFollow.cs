using PabloLario.Managers;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using System.Collections;
using PabloLario.DungeonGeneration;
using System.Text;

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
		protected bool followingPath = true;

		protected Rigidbody2D _rb;

		protected virtual void Awake()
        {
			_roomAssociatedTo = FindObjectOfType<Room>();
			timeBetweenMovesCounter = timeBetweenMoves;
			_rb = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
			base.Start();

			StartCoroutine(Co_Move());
        }

        private void FixedUpdate()
        {
			if (followingPath)
				return;

			Vector2 playerDirNorm = (Assets.Instance.playerTransform.position - transform.position).normalized;
			_rb.MovePosition(_rb.position + (enemyMoveSpeed * Time.fixedDeltaTime * playerDirNorm));
			transform.up = playerDirNorm;
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
			// Checking if there are walls between
			Vector2 hitDir = Assets.Instance.playerTransform.position - transform.position;
			LayerMask unwalkableMask = LayerMask.GetMask(new string[] { "Unwalkable", "RoomExplored", "RoomUnexplored" });
			RaycastHit2D hit = Physics2D.Raycast(transform.position, hitDir.normalized, hitDir.magnitude, unwalkableMask);
			if (hit.collider == null)
				followingPath = false;
			else
				followingPath = true;

			if (_roomAssociatedTo != null)
				_roomAssociatedTo.pathRequestManager.RequestPath(transform.position, Assets.Instance.playerTransform.position, OnPathFound);
			else
				print("Room Associated To is null");

			yield return new WaitForSeconds(.15f);

			yield return Co_Move();
		}

        public override void Die()
        {
            GameManager.InvokeDelegateEnemyDead(abilityPointsToGiveToPlayerWhenDied);
            _roomAssociatedTo.ReduceEnemyCounter();

			GameObject deadParticles = Instantiate(ParticlesManager.Instance.GetParticles(ParticleType.EnemyDead), transform.position, Quaternion.identity, Assets.Instance.splashContainer);
			deadParticles.GetComponent<ParticleSystem>().startColor = hitAnimation.agentColor;
			Destroy(Instantiate(deadParticles, transform.position, transform.rotation), 0.5f);
            
			Instantiate(Assets.Instance.bloodSplash1, transform.position, transform.rotation, Assets.Instance.splashContainer);
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
			if (!followingPath)
            {
				Vector2 hitDir = Assets.Instance.playerTransform.position - transform.position;
				Gizmos.DrawRay(transform.position, hitDir);
				return;
            }

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
