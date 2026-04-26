using Godot;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http.Headers;

public partial class AgentIdleState : AgentState
{
	[Export] private Node _wanderState;

	[ExportCategory("Idle time")]
	[Export] private float _minIdleTime;
	[Export] private float _maxIdleTime;

	private float _idleTime;

    public override void OnStateEnter()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();

		_idleTime = rng.RandfRange(_minIdleTime, _maxIdleTime);
    }

    public override void OnStateExit()
    {
        _idleTime = 0;
    }

    public override void OnStateUpdate(float delta)
    {
		if(_idleTime < 0)
		{
			_agentStateManager.ChangeState(_wanderState);
			return;
		}

        _idleTime -= delta;
    }

}
