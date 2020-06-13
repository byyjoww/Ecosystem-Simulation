using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class BaseState : IState
    {
        public event Action OnEnterState;
        public event Action OnExitState;

        public virtual void EnterState(FiniteStateMachine fsm)
        {
            OnEnterState?.Invoke();
        }

        public virtual void StateUpdate(FiniteStateMachine fsm)
        {
            if (StateToTransition() != null)
            {
                ExitState(fsm, StateToTransition());
            }
        }

        public virtual void ExitState(FiniteStateMachine fsm, IState stateToTransition)
        {
            OnExitState?.Invoke();
        }

        public abstract IState StateToTransition();
    }
}