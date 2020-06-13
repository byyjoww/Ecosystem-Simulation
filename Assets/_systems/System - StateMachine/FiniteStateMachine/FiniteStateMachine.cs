using UnityEngine;
using FSM;

namespace FSM
{
    public class FiniteStateMachine : MonoBehaviour
    {
        public string CurrentState => currentState.ToString();
        //--------STATES--------
        private IState currentState { get; set; }

        IState preparationState = new Preparation();
        IState matchStartState = new MatchStart();
        IState mainState = new Main();
        IState matchEndState = new MatchEnd();

        public void Start()
        {
            SetIdleState(preparationState);
        }

        public void Update()
        {
            currentState.StateUpdate(this);
        }

        public void SetIdleState(IState state)
        {
            currentState = state;
        }

        public void TransitionToState(IState state)
        {
            currentState = state;
            state.EnterState(this);
        }
    }
}