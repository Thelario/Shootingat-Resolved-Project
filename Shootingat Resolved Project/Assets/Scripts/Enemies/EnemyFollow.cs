using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : EnemyBase
{
    [Header("Enemy Stats")]
    [SerializeField] private float enemyMoveSpeed;

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IDamageable id) && collision.collider.CompareTag("Enemy") == false)
        {
            id.TakeDamage(1);
        }
    }

    /// <summary>
    /// Moves enemy with a certain speed so that it chases the player
    /// </summary>
    private void Move()
    {
        Vector3 dir = Assets.Instance.playerTransform.position - transform.position;
        transform.position = transform.position + dir.normalized * enemyMoveSpeed * Time.deltaTime;

        Rotate(dir);
    }

    /// <summary>
    /// Rotates enemy so that it points to the player
    /// </summary>
    /// <param name="dir"> Direction to point to </param>
    private void Rotate(Vector3 dir)
    {
        transform.up = dir;
    }

    /// <summary>
    /// All necessary actions to happen when an enemy dies
    /// </summary>
    public override void Die()
    {
        //OnEnemyDead(clarityToGiveToPlayerWhenDied);
        RoomAssociatedTo.ReduceEnemyCounter();
        Destroy(Instantiate(ParticlesManager.Instance.GetParticles(ParticleType.EnemyDead), transform.position, transform.rotation), 0.5f);
        Destroy(Instantiate(Assets.Instance.bloodSplash_1, transform.position, transform.rotation), 10f);
        Destroy(gameObject);
    }
}
