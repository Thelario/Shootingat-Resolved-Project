using UnityEngine;
using PabloLario.StateMachine;

namespace PabloLario.Characters.Boss
{
    public class BossEnrage : State
    {
        private BossStateMachine _bsm;

        public BossEnrage(BossStateMachine bsm) : base(bsm)
        {
            _bsm = bsm;
        }

        public override void Enter()
        {
            _bsm.Enraged = true;
            _bsm.Renderer.sprite = _bsm.enragedBossSprite;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}