using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Enemies
{
    public class EnemyParallelShoot : EnemyFollow
    {
        [Header("References")]
        [SerializeField] private EnemyBulletStats bulletStats;
        [SerializeField] private Transform weapon;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform innerShootPoint;

        [Header("Inner Circle Animation")]
        [SerializeField] private GameObject innerCircle;
        [SerializeField] private Vector3 scaleTo;
        [SerializeField] private Vector3 defaultScale;
        [SerializeField] private float scaleTime;

        [Header("Fields")]
        [SerializeField] private float shootTime;
        [SerializeField] private float shootDst;
        [SerializeField] private int numberOfBullets;

        private float _shootTimeCounter;

        protected override void Start()
        {
            base.Start();

            _shootTimeCounter = 0f;
        }

        private void LateUpdate()
        {
            Vector2 dir = Assets.Instance.playerTransform.position - transform.position;
            weapon.transform.up = dir.normalized;

            if (!PlayerOnZone())
                return;

            CheckShoot();
        }

        private void CheckShoot()
        {
            _shootTimeCounter -= Time.deltaTime;

            if (_shootTimeCounter <= 0f)
            {
                Shoot();
                _shootTimeCounter = shootTime;
            }
        }

        private void Shoot()
        {
            AnimateInnerCircle();

            float currentRate = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0) 
                    currentRate = -15f;
                else 
                    currentRate = 20f;

                shootPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, currentRate + shootPoint.rotation.eulerAngles.z));

                Vector3 dir = innerShootPoint.position - shootPoint.position;

                GameObject g = BulletPoolManager.Instance.RequestEnemyBullet();
                g.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);

                Bullet b = g.GetComponent<Bullet>();
                b.SetDirStatsColor((Vector2)dir.normalized, bulletStats, hitAnimation.agentColor);

                ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, innerShootPoint.rotation);
            }

            shootPoint.rotation = weapon.rotation;
        }

        private void AnimateInnerCircle()
        {
            LeanTween.scale(innerCircle, scaleTo, scaleTime);
            LeanTween.scale(innerCircle, defaultScale, scaleTime).setDelay(scaleTime);
        }

        private bool PlayerOnZone()
        {
            Vector3 dir = Assets.Instance.playerTransform.position - transform.position;

            if (dir.magnitude < shootDst)
                return true;
            else
                return false;
        }
    }
}