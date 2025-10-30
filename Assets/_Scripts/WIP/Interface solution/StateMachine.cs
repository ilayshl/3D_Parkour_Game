using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    StateNode currentState;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransitions = new();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }
        
            currentState.State?.Update();
    }

    public void FixedUpdate()
    {
        currentState.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        currentState = nodes[state.GetType()];
        currentState.State.OnEnter();
    }

    private void ChangeState(IState state)
    {
        if (state == currentState.State) return;

        var previousState = currentState.State;
        var newState = nodes[state.GetType()].State;

        previousState?.OnExit();
        newState?.OnEnter();
        currentState = nodes[state.GetType()];
    }

    private ITransition GetTransition()
    {
        foreach (var transition in anyTransitions)
        {
            if (transition.Condition.Evaluate())
            {
                return transition;
            }
        }

        foreach (var transition in currentState.Transitions)
        {
            if (transition.Condition.Evaluate())
            {
                return transition;
            }
        }

        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    private StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }
    private class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}
