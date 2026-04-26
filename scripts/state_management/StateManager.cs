using Godot;
using System;
using System.Diagnostics;

public partial class StateManager : Node2D
{
	[Export] protected Node _initialState;
	[Export] private bool _changeToInitialStateOnReady;

	private IState _currentState;
	protected IState CurrentState { get => _currentState; }

    public override void _Ready()
    {
		if (_changeToInitialStateOnReady)
		{
			ChangeToInitialState();
		}
    }

	protected void ChangeToInitialState()
	{
		if(_currentState == null)
		{
			ChangeState(_initialState);
		}
	}

    public override void _Process(double delta)
    {
		if(_currentState != null)
		{
        	_currentState.OnStateUpdate((float)delta);
		}
    }

	public void ChangeState(Node newState)
	{
		if(newState is IState)
		{
			Debug.Print("Changing to " + newState.Name);
			ChangeState((IState)newState);
		}
	}

	public void ChangeState(IState newState)
	{
		if(_currentState != null)
		{
			_currentState.OnStateExit();
		}
		_currentState = newState;
		_currentState.OnStateEnter();
	}
}
