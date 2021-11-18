using UnityEngine;
using PabloLario.StateMachine;

namespace PabloLario.Characters.Boss
{
    public class BossEnrage : State
    {
        private float _timeInEnrageBeforeStartMovingCounter;

        private BossStateMachine _bsm;

        public BossEnrage(BossStateMachine bsm) : base(bsm)
        {
            _bsm = bsm;
        }

        public override void Enter()
        {
            _bsm.BossStats.enraging = true;
            _bsm.Enraged = true;
            _bsm.Renderer.sprite = _bsm.enragedBossSprite;
            _bsm.BossStats.hitAnimation.agentColor = _bsm.bossEnragedColor;
            _timeInEnrageBeforeStartMovingCounter = _bsm.BossStats.timeInEnrageBeforeStartMoving;
            _bsm.Animator.Play(_bsm.ENRAGE);
        }

        public override void Exit()
        {
            _bsm.BossStats.enraging = false;
        }

        public override void Update()
        {
            _timeInEnrageBeforeStartMovingCounter -= Time.deltaTime;
            if (_timeInEnrageBeforeStartMovingCounter <= 0f)
            {
                _bsm.ChangeState(_bsm.BossMove);
            }
        }
    }
}