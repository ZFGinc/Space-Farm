using System;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<TState, TEvent>
{
    private Dictionary<TState, Dictionary<TEvent, TState>> _transitions;
    private TState _currentState;

    public TState CurrentState => _currentState;

    public FiniteStateMachine(TState initialState)
    {
        _transitions = new Dictionary<TState, Dictionary<TEvent, TState>>();
        _currentState = initialState;
    }

    public void AddTransition(TState fromState, TEvent @event, TState toState)
    {
        if (!_transitions.ContainsKey(fromState))
        {
            _transitions[fromState] = new Dictionary<TEvent, TState>();
        }
        _transitions[fromState][@event] = toState;
    }

    public void Trigger(TEvent @event)
    {
        if (_transitions.ContainsKey(_currentState) && _transitions[_currentState].ContainsKey(@event))
        {
            _currentState = _transitions[_currentState][@event];
        }
        else
        {
            // Обработка ситуации, когда нет перехода для данного события
            Debug.Log($"Нет перехода для события {@event} в состоянии {_currentState}");
        }
    }
}