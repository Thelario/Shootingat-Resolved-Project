using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootBasic : EnemyBase
{
    [Header("Enemy Shoot Fields")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootTime;
    [SerializeField] private Transform weapon;
    [SerializeField] private Animator animator;

    private float shootTimeCounter;
    private Vector2 dir;

    public bool playerOnZone = false;
    public Transform target;

    protected override void Start()
    {
        base.Start();
        shootTimeCounter = 0f;
    }

    private void LateUpdate()
    {
        if (playerOnZone)
        {
            Rotate(target.position);

            shootTimeCounter += Time.deltaTime;
            if (shootTimeCounter - shootTime >= 0f)
                Shoot();
        }
    }

    public override void Die()
    {
        //OnEnemyDead(clarityToGiveToPlayerWhenDied);
        RoomAssociatedTo.ReduceEnemyCounter();
        Destroy(Instantiate(ParticlesManager.Instance.GetParticles(ParticleType.EnemyDead), transform.position, transform.rotation), 0.5f);
        Destroy(Instantiate(Assets.Instance.bloodSplash_1, transform.position, transform.rotation), 10f);
        Destroy(gameObject);
    }

    private void Rotate(Vector3 target)
    {
        dir = target - transform.position;
        weapon.up = dir;
        animator.SetFloat("Horizontal", dir.normalized.x);
        animator.SetFloat("Vertical", dir.normalized.y);
    }

    private void Shoot()
    {
        shootTimeCounter = 0f;

        GameObject go = BulletPoolManager.Instance.RequestEnemyBullet();
        go.transform.position = shootPoint.position;
        go.transform.rotation = Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z + Random.Range(-5f, 5f));

        // Shooting bullets without Unity Physics System
        Bullet b = go.GetComponent<Bullet>();
        b.SetDir(dir);

        // Shooting bullets with Unity Physics System
        // Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        // rb.AddForce(go.transform.up * bulletSpeed, ForceMode2D.Impulse);

        ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);
    }
}
