using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransitionStateMachine<TState> : MonoBehaviour
{
    protected TState state = default;
    protected abstract TState DefaultState { get; }

    #region TRANSITION_CLASS
    protected class Transition
    {
        public readonly TState input;
        public readonly TState output;
        public readonly Func<bool> predicate;
        public readonly Action action;

        public Transition(TState input, TState output, Func<bool> predicate, Action action = null)
        {
            this.input = input;
            this.output = output;
            this.predicate = predicate;
            this.action = action;
        }
    }
    #endregion

    #region INITIALIZE

    /// <summary>
    /// Set the StateDictionary and TransitionList on initialize
    /// </summary>
    protected abstract Dictionary<TState, Action> StateDictionary { get; set; }
    protected abstract List<Transition> TransitionList { get; set; }
    protected virtual void Awake()
    {
        Initialize();
    }

    protected abstract void Initialize();
    #endregion

    #region CHANGE_STATES
    protected float TimeOnState { get; private set; }

    protected virtual void Update()
    {
        Tick();
        TimeOnState += Time.deltaTime;
    }

    private void Tick()
    {
        TimeOnState += Time.deltaTime;

        foreach (Transition transition in TransitionList)
        {
            if (transition.input.Equals(state) && transition.predicate.Invoke())
            {
                state = transition.output;
                transition.action?.Invoke();
                TimeOnState = 0;
                break;
            }
        }

        StateDictionary[state]?.Invoke();
    }
    #endregion
}