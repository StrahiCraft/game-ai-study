using Godot;
using System;
using System.Diagnostics;

public partial class AgentChasingPlayerState : AgentState
{
	[Export] private float _minDistanceToTarget;
	[Export] private Node _wanderState;

    public override void OnPlayerSpotted()
    {
        
    }


    public override void OnStateEnter()
    {
        AgentManager.Instance.OnPlayerSpotted();
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate(float delta)
    {
        _agentStateManager.SetTargetPosition(AgentManager.Instance.PlayerPosition);
		_agentStateManager.AvoidObstacles();
		_agentStateManager.Steer(delta);

		if (_agentStateManager.DistanceToTarget() > _minDistanceToTarget)
		{
			return;
		}

		if (!AgentManager.Instance.PlayerSpotted)
		{
			_agentStateManager.ChangeState(_wanderState);
		}
    }
}
