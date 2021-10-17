using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;
using System.Collections;
using UnityEngine;

namespace PabloLario.Characters.Enemies
{
    public class EnemyShootBasic : EnemyFollow
    {
        [Header("Enemy Shoot Fields")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private EnemyBulletStats bulletStats;
        [SerializeField] private float shootTime;
        [SerializeField] private Transform weapon;
        [SerializeField] private Animator animator;
        [SerializeField] private float minDistanceAwayFromPlayer;
        [SerializeField] private float shootDst;


        private float shootTimeCounter;
        private Vector2 dir;
        private bool moving = true;

        protected override void Start()
        {
            base.Start();
            shootTimeCounter = 0f;
        }

        private void LateUpdate()
        {
            if (!PlayerOnZone())
                return;

            shootTimeCounter += Time.deltaTime;

            if (shootTimeCounter - shootTime >= 0f)
            {
                StartCoroutine(Shoot());
            }
            else if (shootTimeCounter - shootTime >= shootTime / 5f)
            {
                moving = false;
            }
        }

        protected override IEnumerator Co_Move()
        {
            if (!moving)
                yield break;

            Vector3 dir = a.playerTransform.position - transform.position;
            if (dir.magnitude < minDistanceAwayFromPlayer)
                yield break; ;

            this.dir = dir.normalized;
            StartCoroutine(base.Co_Move());
        }

        private IEnumerator Shoot()
        {
            shootTimeCounter = 0f;

            GameObject go = BulletPoolManager.Instance.RequestEnemyBullet();
            go.transform.position = shootPoint.position;
            go.transform.rotation = Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z + Random.Range(-5f, 5f));

            Bullet b = go.GetComponent<Bullet>();
            b.SetDirAndStats(dir, bulletStats);


            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);

            yield return new WaitForSeconds(1f);
            moving = true;
        }

        private bool PlayerOnZone()
        {
            Vector3 dir = a.playerTransform.position - transform.position;

            if (dir.magnitude < shootDst)
                return true;
            else
                return false;
        }
    }
}