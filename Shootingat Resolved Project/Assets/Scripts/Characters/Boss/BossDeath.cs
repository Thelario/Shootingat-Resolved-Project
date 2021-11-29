using PabloLario.StateMachine;
using System.Collections;
using System.Collections.Generic;
using PabloLario.Managers;
using UnityEngine;

namespace PabloLario.Characters.Boss
{
    public class BossDeath : State
    {
        private BossStateMachine _bsm;

        public BossDeath(BossStateMachine bsm) : base(bsm)
        {
            _bsm = bsm;
        }

        public override void Enter()
        {
            _bsm.Animator.Play(_bsm.DIE);
            GameObject.Destroy(_bsm.gameObject, 2f);
            GameManager.InvokeDelegateOnWinGame();
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}