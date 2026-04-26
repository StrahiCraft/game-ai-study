using Godot;

public partial class AgentWanderState : AgentState
{
	[Export] private Node _idleState;
	[Export] private Node _chasePlayerState;

	[ExportCategory("Wander time settings")]
	[Export] private float _minWanderTime;
	[Export] private float _maxWanderTime;

	[ExportCategory("Wander settings")]
	[Export] private float _fov;
	[Export] private float _wanderPointDistance;
	[Export] private float _minDistanceToTarget;

	private float _wanderTime;

	protected void RandomizeTargetPosition()
	{
		_agentStateManager.SetTargetPosition(GenerateRandomTargetPosition());
	}

	protected Vector2 GenerateRandomTargetPosition()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();

		return new Vector2(Mathf.Sin(Mathf.DegToRad(rng.RandfRange(_agentStateManager.RotationDegrees - _fov / 2 + 90,
			_agentStateManager.RotationDegrees + _fov / 2 + 90))),
            Mathf.Cos(Mathf.DegToRad(rng.RandfRange(_agentStateManager.RotationDegrees - _fov / 2 + 90,
			_agentStateManager.RotationDegrees + _fov / 2 + 90)))) *
			_wanderPointDistance + _agentStateManager.Position;
	}

    public override void OnStateEnter()
    {
		RandomNumberGenerator rng = new RandomNumberGenerator();
		_wanderTime = rng.RandfRange(_minWanderTime, _maxWanderTime);

    	RandomizeTargetPosition();
    }

    public override void OnStateExit()
    {
		_wanderTime = 0;
    }

    public override void OnStateUpdate(float delta)
    {
		if(_wanderTime < 0)
		{
			_agentStateManager.ChangeState(_idleState);
			return;
		}

		_wanderTime -= delta;
		_agentStateManager.AvoidObstacles();
		_agentStateManager.Steer(delta);

        if(_agentStateManager.DistanceToTarget() <= _minDistanceToTarget)
		{
			RandomizeTargetPosition();
		}
    }

    public override void OnPlayerSpotted()
    {
        _agentStateManager.ChangeState(_chasePlayerState);
    }
}
