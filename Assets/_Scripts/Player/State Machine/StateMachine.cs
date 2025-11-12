using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state machine that handles possible PlayerStates from its Factory.
/// Also checks for transitions that should always be checked.
/// </summary>
public class StateMachine
{
    public PlayerState CurrentState => _currentState;
    public PlayerStateFactory Factory => _stateFactory;

    private PlayerControllerFacade _playerManager;
    private PlayerState _currentState;
    private PlayerStateFactory _stateFactory;
    private List<StatePredicate> anyTransitions = new();

    public StateMachine(PlayerControllerFacade playerManager)
    {
        _playerManager = playerManager;
        _stateFactory = new PlayerStateFactory(this, _playerManager);
    }

    public void Start()
    {
        ChangeState(_stateFactory.Airborne()); //Default state
    }

    public void Update()
    {
        _currentState?.Update();
        CheckAnyTransitions();
    }

    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    /// <summary>
    /// Iterates through anyTransitions to find a condition that returns true.
    /// </summary>
    private void CheckAnyTransitions()
    {
        if (anyTransitions.Count == 0) return;

        foreach (var transition in anyTransitions)
        {
            if (transition.Evaluate() && transition.To != CurrentState)
            {
                ChangeState(transition.To);
                return;
            }
        }
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
        Debug.Log("Changed to state " + newState);
    }

    /// <summary>
    /// Adds to the list of transitions that should be checked every frame, able to override other states.
    /// </summary>
    /// <param name="to"></param>
    /// <param name="predicate"></param>
    public void AddAnyTransition(PlayerState to, Func<bool> predicate)
    {
        StatePredicate transition = new(to, predicate);
        if (!anyTransitions.Contains(transition))
        {
            anyTransitions.Add(transition);
        }
        else
        {
            Debug.LogWarning($"[StateMachine] AnyTransitions list already contains {transition.ToString()}");
        }
        //I could do 'bool result = contains? debug.log : anyTransitions.add' but it would be less readable.
    }

    /// <summary>
    /// A class that holds a state to transition to and a Func<bool> to determine whether to transition or not.
    /// </summary>
    private class StatePredicate
    {
        public readonly PlayerState To;
        private Func<bool> _predicate;
        public StatePredicate(PlayerState toState, Func<bool> predicate)
        {
            To = toState;
            _predicate = predicate;
        }

        public bool Evaluate()
        {
            return _predicate.Invoke();
        }

        public override string ToString()
        {
            return $"StatePredicate to {To.GetType().Name}";
        }

    }

}

