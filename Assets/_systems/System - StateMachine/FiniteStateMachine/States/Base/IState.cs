using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FSM
{
    public interface IState
    {
        event Action OnEnterState;
        event Action OnExitState;

        void EnterState(FiniteStateMachine fsm);

        void StateUpdate(FiniteStateMachine fsm);

        void ExitState(FiniteStateMachine fsm, IState stateToTransition);

        IState StateToTransition();
    }
}