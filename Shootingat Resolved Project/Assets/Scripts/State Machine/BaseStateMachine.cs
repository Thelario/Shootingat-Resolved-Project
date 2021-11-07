using UnityEngine;

namespace PabloLario.StateMachine
{
    public abstract class BaseStateMachine : MonoBehaviour
    {
        public State CurrentState { get; protected set; }

        public void ChangeState(State newState)
        {
            if (newState == null)
                Debug.LogError("Passing a state that is null. State: " + newState);

            CurrentState?.Exit();

            CurrentState = newState;

            CurrentState?.Enter();
        }
    }
}