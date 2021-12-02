using PabloLario.Characters.Core.Shooting;
using PabloLario.Managers;
using System.Collections;
using UnityEngine;


#pragma warning disable CS0618 // startColor in ParticleSystem is obsolete

namespace PabloLario.Characters.Enemies
{
    public class EnemyBurst : Enemy
    {
        [Header("References")]
        [SerializeField] private Transform weapon;
        [SerializeField] private Transform shootPoint;

        [Header("Inner Circle Animation")]
        [SerializeField] private GameObject innerCircle;
        [SerializeField] private Vector3 scaleTo;
        [SerializeField] private Vector3 defaultScale;
        [SerializeField] private float scaleTime;

        [Header("Enemy Bullet Stats")]
        [SerializeField] private EnemyBulletStats bulletStats;

        [Header("Enemy Burst Fields")]
        [SerializeField] private float minRotation;
        [SerializeField] private float maxRotation;
        [SerializeField] private int numberOfBullets;
        [SerializeField] private bool isRandom;
        [SerializeField] private float shootTime;

        [Header("Enemy Consecutive Burst Fields")]
        [SerializeField] private bool consecutiveShooting = false;
        [SerializeField] private float consecutiveShootTime = 0.25f;

        [Header("Enemy Rotation")]
        [SerializeField] private float rotationSpeed;

        private float _shootTimeCounter;
        private float _rotationTimeCounter;

        protected override void Start()
        {
            base.Start();

            _shootTimeCounter = 0f;
            _rotationTimeCounter = 0f;
        }

        private void Update()
        {
            // transform.RotateAroundLocal(Vector3.forward, rotationSpeed * Time.deltaTime);
            _rotationTimeCounter += Time.deltaTime * rotationSpeed;

            _shootTimeCounter -= Time.deltaTime;

            if (_shootTimeCounter > 0)
                return;

            StartCoroutine(ShootBullets());
            _shootTimeCounter = shootTime;
        }

        private IEnumerator ShootBullets()
        {
            if (consecutiveShooting)
                AnimateInnerCircle();

            float rate = 360f / numberOfBullets;
            float currentRate = rate;
            for (int i = 0; i < numberOfBullets; i++)
            {
                weapon.rotation = Quaternion.Euler(new Vector3(0f, 0f, currentRate + _rotationTimeCounter));

                Vector3 dir = shootPoint.position - transform.position;

                GameObject g = BulletPoolManager.Instance.RequestEnemyBullet();
                g.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);

                Bullet b = g.GetComponent<Bullet>();
                b.SetDirStatsColor((Vector2)dir.normalized, bulletStats, hitAnimation.agentColor);

                ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);

                currentRate += rate;

                if (consecutiveShooting)
                    yield return new WaitForSeconds(consecutiveShootTime);
            }

            AnimateInnerCircle();

            yield return null;
        }

        private void AnimateInnerCircle()
        {
            LeanTween.scale(innerCircle, scaleTo, scaleTime);
            LeanTween.scale(innerCircle, defaultScale, scaleTime).setDelay(scaleTime);
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
    }
}