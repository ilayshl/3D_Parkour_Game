using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public PlayerState CurrentState => _currentState;
    public PlayerStateFactory Factory => _stateFactory;

    private PlayerManager _playerManager;
    private PlayerState _currentState;
    private PlayerStateFactory _stateFactory;
    private List<StatePredicate> anyTransitions = new();

    public StateMachine(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _stateFactory = new PlayerStateFactory(this, _playerManager);
    }

    public void Start()
    {
        ChangeState(_stateFactory.Airborne());
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

    private void CheckAnyTransitions()
    {
        if (anyTransitions.Count == 0) return;

        foreach(var transition in anyTransitions)
        {
            if(transition.Evaluate() && transition.To != CurrentState)
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

    public void AddAnyTransition(StatePredicate transition)
    {
        if (!anyTransitions.Contains(transition))
        {
            anyTransitions.Add(transition);
        }
        else
        {
            Debug.LogWarning($"[StateMachine] AnyTransitions list already contains {transition.ToString()}");
        }
    }
    
}

public class StatePredicate
{
    public PlayerState To => _to;
    private PlayerState _to;
    private Func<bool>_predicate;
    public StatePredicate(PlayerState toState, Func<bool> predicate)
    {
        _to = toState;
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
