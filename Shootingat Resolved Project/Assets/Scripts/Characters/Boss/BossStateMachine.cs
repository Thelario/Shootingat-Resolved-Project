using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using PabloLario.StateMachine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Boss
{
    public class BossStateMachine : BaseStateMachine
    {
        public Color bossEnragedColor;
        public Sprite defaultBossSprite;
        public Sprite enragedBossSprite;
        public Transform shootPoint;
        public Transform weapon;

        public Transform CurrentNode { get; set; }

        public Rigidbody2D Rb { get; private set; }

        public Transform Tr { get; private set; }

        public BossStats BossStats { get; private set; }

        public Animator Animator { get; private set; }

        public SpriteRenderer Renderer { get; private set; }

        public bool Enraged { get; set; }

        // Boss stats should probably be passed to each state. Maybe the best way to do it
        // is in the constructor. I could simply modify the constructor for each state with
        // the addition of the BossStats.
        // [Serialized] private BossStats stats; // BossStats needs to be implemented

        public BossStart BossStart { get; private set; }
        public BossMove BossMove { get; private set; }
        public BossStop BossStop { get; private set; }
        public BossEnrage BossEnrage { get; private set; }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Tr = transform;
            BossStats = GetComponent<BossStats>();
            Animator = GetComponent<Animator>();
            Renderer = Tr.GetChild(0).GetComponent<SpriteRenderer>();

            BossStart = new BossStart(this);
            BossMove = new BossMove(this);
            BossStop = new BossStop(this);
            BossEnrage = new BossEnrage(this);

            BossStats.OnBossEnrage += Enrage;
        }

        private void OnDestroy()
        {
            BossStats.OnBossEnrage -= Enrage;
        }

        private void Start()
        {
            CurrentState = BossStart;
            CurrentState?.Enter();
        }

        private void Update()
        {
            CurrentState?.Update();
        }

        public IEnumerator BurstShooting()
        {
            float fraction = 360f / BossStats.numberOfBulletsInWavesWhenMoving;

            if (Enraged)
            {
                float randomFraction = Random.Range(0f, 30f);
                fraction += randomFraction;
            }

            for (int i = 0; i < BossStats.numberOfBulletsInWavesWhenMoving; i++)
            {
                weapon.Rotate(new Vector3(0f, 0f, fraction));
                Vector2 dir = shootPoint.position - weapon.position;

                GameObject g = BulletPoolManager.Instance.RequestEnemyBullet();
                g.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);

                Bullet b = g.GetComponent<Bullet>();
                b.SetDirStatsColor(dir.normalized, BossStats.bulletStats, BossStats.hitAnimation.agentColor);

                ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);

                yield return new WaitForSeconds(BossStats.timeBetweenBulletsInBurstShooting);
            }
        }

        private void Enrage()
        {
            ChangeState(BossEnrage);
        }
    }
}