using UnityEngine;
using PabloLario.StateMachine;

namespace PabloLario.Characters.Boss
{
    public class BossStart : State
    {
        private float timeTillBossBattleStart = 4f;
        private float timeTillBossBattleStartCounter;

        private BossStateMachine _bsm;

        public BossStart(BossStateMachine bsm) : base(bsm)
        {
            _bsm = bsm;
        }

        public override void Enter()
        {
            _bsm.Enraged = false;
            timeTillBossBattleStartCounter = timeTillBossBattleStart;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            timeTillBossBattleStartCounter -= Time.deltaTime;

            if (timeTillBossBattleStartCounter <= 0f)
                _bsm.ChangeState(_bsm.BossMove);
        }
    }
}