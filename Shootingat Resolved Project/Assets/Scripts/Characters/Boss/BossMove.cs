using PabloLario.Characters.Core.Shooting;
using PabloLario.Managers;
using PabloLario.StateMachine;
using UnityEngine;

namespace PabloLario.Characters.Boss
{
    public class BossMove : State
    {
        private Vector3 dirNorm = Vector3.zero;
        private float timeBetweenWavesWhenMovingCounter;

        private BossStateMachine _bsm;

        public BossMove(BossStateMachine bsm) : base(bsm) 
        {
            _bsm = bsm;
        }

        public override void Enter()
        {
            if (_bsm.Enraged)
                _bsm.CurrentNode = BossRoomNodes.Instance.GetClosestNodeToPlayer();
            else
                _bsm.CurrentNode = BossRoomNodes.Instance.GetRandomNode();

            timeBetweenWavesWhenMovingCounter = 0f;
            dirNorm = (_bsm.CurrentNode.position - _bsm.Tr.position).normalized;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            // Check if dirmNorm has already been set. It doesn't give a problem with the center node because it is not in 0,0.
            if (dirNorm == Vector3.zero)
                return;

            CheckRemainingDst();

            CheckWaveShooting();

            Move();
        }

        private void Move()
        {
            _bsm.Tr.position += _bsm.BossStats.moveSpeed * Time.deltaTime * dirNorm;
        }

        private void CheckRemainingDst()
        {
            if (Mathf.Abs((_bsm.CurrentNode.position - _bsm.Tr.position).magnitude) <= 0.15f)
                _bsm.ChangeState(_bsm.BossStop);
        }

        private void CheckWaveShooting()
        {
            timeBetweenWavesWhenMovingCounter -= Time.deltaTime;

            if (timeBetweenWavesWhenMovingCounter <= 0f)
            {
                timeBetweenWavesWhenMovingCounter = _bsm.BossStats.timeBetweenWavesWhenMoving;
                WaveShooting();
            }
        }

        private void WaveShooting()
        {
            float fraction = 360f / _bsm.BossStats.numberOfBulletsInWavesWhenMoving;

            if (_bsm.Enraged)
            {
                float randomFraction = Random.Range(0f, 30f);
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

                //currentRate += rate;
            }
        }
    }
}