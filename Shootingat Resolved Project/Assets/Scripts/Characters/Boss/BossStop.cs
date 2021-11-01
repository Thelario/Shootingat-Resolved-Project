using UnityEngine;
using PabloLario.StateMachine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;
using System.Collections;

namespace PabloLario.Characters.Boss
{
    public class BossStop : State
    {
        private float timeBetweenBulletCounter;
        private float timeElapsedInStop = 0f;

        private BossStateMachine _bsm;

        public BossStop(BossStateMachine bsm) : base(bsm) 
        {
            _bsm = bsm;
        }

        public override void Enter()
        {
            timeElapsedInStop = 0f;
            timeBetweenBulletCounter = _bsm.BossStats.timeInStopTillStartShooting;
        }

        public override void Exit()
        {
            timeElapsedInStop = 0f;
        }

        public override void Update()
        {
            timeElapsedInStop += Time.deltaTime;

            CheckBurstShooting();
        }

        private void CheckBurstShooting()
        {
            timeBetweenBulletCounter -= Time.deltaTime;

            if (timeBetweenBulletCounter <= 0f)
            {
                timeBetweenBulletCounter = _bsm.BossStats.timeBetweenWavesWhenMoving;

                if (timeElapsedInStop <= 10f)
                    BurstShooting();
                else if (timeElapsedInStop <= 13f)
                    return;
                else if (timeElapsedInStop <= 20f)
                    WaveShooting();
                else if (timeElapsedInStop <= 23f)
                    return;
                else if (timeElapsedInStop <= 30f)
                    BurstShooting();
                else if (timeElapsedInStop > 30f)
                    _bsm.ChangeState(_bsm.BossMove);
            }
        }

        private void BurstShooting()
        {
            timeBetweenBulletCounter = _bsm.BossStats.timeBetweenBursts;

            _bsm.StartCoroutine(_bsm.BurstShooting());
        }

        private void WaveShooting()
        {
            timeBetweenBulletCounter = _bsm.BossStats.timeBetweenWavesWhenStopped;

            float fraction = 360f / _bsm.BossStats.numberOfBulletsInWavesWhenMoving;

            if (_bsm.Enraged)
            {
                float randomFraction = Random.Range(0f, 10f);
                fraction += randomFraction;
            }

            for (int i = 0; i < _bsm.BossStats.numberOfBulletsInWavesWhenMoving; i++)
            {
                _bsm.weapon.Rotate(new Vector3(0f, 0f, fraction));
                Vector2 dir = _bsm.shootPoint.position - _bsm.weapon.position;

                GameObject g = BulletPoolManager.Instance.RequestEnemyBullet();
                g.transform.SetPositionAndRotation(_bsm.shootPoint.position, _bsm.shootPoint.rotation);

                Bullet b = g.GetComponent<Bullet>();
                b.SetDirStatsColor(dir.normalized, _bsm.BossStats.bulletStats, _bsm.BossStats.hitAnimation.agentColor);

                ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, _bsm.shootPoint.position, 0.5f, _bsm.shootPoint.rotation);
            }
        }
    }
}