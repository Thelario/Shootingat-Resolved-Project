using PabloLario.Characters.Core.Shooting;
using PabloLario.Managers;
using System.Collections;
using UnityEngine;

namespace PabloLario.Characters.Enemies
{
    public enum EnemyShootType
    {
        OneShot,
        DoubleContinuousShot,
        TripleContinuousShot
    }

    public class EnemyShoot : EnemyFollow
    {
        [Header("Enemy Shoot Fields")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private EnemyBulletStats bulletStats;
        [SerializeField] private float shootTime;
        [SerializeField] private Transform weapon;
        [SerializeField] private Animator animator;
        [SerializeField] private float minDistanceAwayFromPlayer;
        [SerializeField] private float shootDst;
        [SerializeField] private EnemyShootType enemyShootType;
        [SerializeField] private SpriteRenderer sr;

        private float _shootTimeCounter;
        private Vector2 _dir;
        private bool _moving = true;

        protected override void Start()
        {
            base.Start();
            _shootTimeCounter = 0f;
        }

        private void LateUpdate()
        {
            if (!PlayerOnZone())
                return;

            StartCoroutine(CheckShoot());
        }

        private IEnumerator CheckShoot()
        {
            _shootTimeCounter += Time.deltaTime;

            if (_shootTimeCounter - shootTime >= 0f)
            {
                switch (enemyShootType)
                {
                    case EnemyShootType.OneShot:
                        Shoot();
                        break;
                    case EnemyShootType.DoubleContinuousShot:
                        Shoot();
                        yield return new WaitForSeconds(.2f);
                        Shoot();
                        break;
                    case EnemyShootType.TripleContinuousShot:
                        Shoot();
                        yield return new WaitForSeconds(.2f);
                        Shoot();
                        yield return new WaitForSeconds(.2f);
                        Shoot();
                        break;
                }

                yield return new WaitForSeconds(1f);

                _moving = true;
            }
            else if (_shootTimeCounter - shootTime >= shootTime / 5f)
            {
                _moving = false;
            }
        }

        protected override IEnumerator Co_Move()
        {
            if (!_moving)
                yield break;

            Vector3 dir = a.playerTransform.position - transform.position;
            if (dir.magnitude < minDistanceAwayFromPlayer)
                yield break; ;

            this._dir = dir.normalized;

            StartCoroutine(base.Co_Move());
        }

        private void Shoot()
        {
            _shootTimeCounter = 0f;

            GameObject go = BulletPoolManager.Instance.RequestEnemyBullet();
            go.transform.SetPositionAndRotation(shootPoint.position, Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z + Random.Range(-5f, 5f)));

            Bullet b = go.GetComponent<Bullet>();
            b.SetDirStatsColor(_dir, bulletStats, hitAnimation.agentColor);

            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);
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