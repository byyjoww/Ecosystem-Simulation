using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI_FSM
{
    public class State
    {
        public enum STATE { IDLE = 0, PATROL = 1, PURSUE = 2, ATTACK = 3 }
        public enum EVENT { ENTER = 0, UPDATE = 1, EXIT = 2 }

        public STATE name;
        protected EVENT stage;
        protected GameObject npc;
        protected Animator anim;
        protected Transform player;
        protected State nextState;
        protected NavMeshAgent agent;

        public State (GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        {
            npc = _npc;
            agent = _agent;
            anim = _anim;
            stage = EVENT.ENTER;
            player = _player;
        }

        public virtual void Enter() { stage = EVENT.ENTER; Debug.Log($"Entered {name} State."); }
        public virtual void Update() { stage = EVENT.UPDATE; Debug.Log($"{name} Update."); }
        public virtual void Exit() { stage = EVENT.EXIT; Debug.Log($"Exited {name} State."); }

        public State Process()
        {
            if (stage == EVENT.ENTER)
            {
                Enter();
            }

            if (stage == EVENT.UPDATE)
            {
                Update();
            }

            if (stage == EVENT.EXIT)
            {
                Exit();
                return nextState;
            }

            return this;
        }
    }
}