using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : Enemy
{
    [Header("Enemy Stats")]
    [SerializeField] private float enemyMoveSpeed;

    protected Assets a;

    protected virtual void Awake()
    {
        a = Assets.Instance;
    }

    private void Update()
    {
        Move();

        Rotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IDamageable id) && collision.collider.CompareTag("Enemy") == false)
        {
            id.TakeDamage(1);
        }
    }

    protected virtual void Move()
    {
        Vector3 dir = a.playerTransform.position - transform.position;
        transform.position = transform.position + dir.normalized * enemyMoveSpeed * Time.deltaTime;
    }

    protected virtual void Rotate()
    {
        Vector3 dir = a.playerTransform.position - transform.position;
        transform.up = dir.normalized;
    }

    public override void Die()
    {
        //OnEnemyDead(clarityToGiveToPlayerWhenDied);
        RoomAssociatedTo.ReduceEnemyCounter();
        Destroy(Instantiate(ParticlesManager.Instance.GetParticles(ParticleType.EnemyDead), transform.position, transform.rotation), 0.5f);
        Destroy(Instantiate(a.bloodSplash_1, transform.position, transform.rotation), 10f);
        Destroy(gameObject);
    }
}
