namespace PabloLario.StateMachine
{
    public abstract class State
    {
        public virtual BaseStateMachine BSM { get; protected set; }

        public State(BaseStateMachine bsm) => BSM = bsm;

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();
    }
}
