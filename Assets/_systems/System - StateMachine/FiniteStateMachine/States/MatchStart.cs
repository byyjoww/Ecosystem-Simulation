using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class MatchStart : BaseState
    {
        public override void EnterState(FiniteStateMachine fsm)
        {
            base.EnterState(fsm);
        }

        public override void StateUpdate(FiniteStateMachine fsm)
        {
            base.StateUpdate(fsm);
        }

        public override void ExitState(FiniteStateMachine fsm, IState stateToTransitionTo)
        {
            base.ExitState(fsm, stateToTransitionTo);
        }

        public override IState StateToTransition()
        {
            throw new NotImplementedException();
        }
    }
}