using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

	private Dictionary<int, State> states;
	private State state;
	private State nextState;

	public StateMachine()
	{
		states = new Dictionary<int, State>();
	}

	public void Update()
	{
		if (nextState != null)
		{
			state?.OnExit?.Invoke();
			state = nextState;
			nextState = null;
			state?.OnEnter?.Invoke();
		}
		state?.OnUpdate?.Invoke();
	}

	public void AddState(int id, Action onEnter, Action onUpdate, Action onExit)
	{
		states[id] = new State(onEnter, onUpdate, onExit);
	}

	public void ChangeToState(int id)
	{
		nextState = states[id];
	}
}

public class State
{
	public Action OnEnter { get; private set; }
	public Action OnUpdate { get; private set; }
	public Action OnExit { get; private set; }

	public State(Action onEnter, Action onUpdate, Action onExit)
	{
		OnEnter = onEnter;
		OnUpdate = onUpdate;
		OnExit = onExit;
	}
}