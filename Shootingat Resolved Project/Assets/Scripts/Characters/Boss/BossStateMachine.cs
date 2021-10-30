using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using PabloLario.StateMachine;

namespace PabloLario.Characters.Boss
{
    public class BossStateMachine : BaseStateMachine
    {
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
            BossStart = new BossStart(this);
            BossMove = new BossMove(this);
            BossStop = new BossStop(this);
            BossEnrage = new BossEnrage(this);
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
    }
}