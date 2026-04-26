using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Godot.Collections;

public partial class Agent : StateManager
{
	[Export] private float _maxSpeed;
	[Export] private float _rotationSpeed;
	[Export(PropertyHint.Layers2DPhysics)] private uint _obstacleMask;
	
	public Vector2 _targetPosition;
	private Vector2 _velocity;

	private List<Node2D> _detectedObstacles = new List<Node2D>();
	private Node2D _detectedPlayer = null;

    public override void _Ready()
    {
        AgentManager.Instance.AddAgent(this);
		base._Ready();
    }

    public override void _Process(double delta)
    {
		SpotPlayer();
        base._Process(delta);
    }


	public void SetTargetPosition(Vector2 targetPosition)
	{
		_targetPosition = targetPosition;
	}

	public void OnPlayerSpotted()
	{
		((AgentState)CurrentState).OnPlayerSpotted();
	}

	public void OnObstacleDetected(Node2D obstacle)
	{
		_detectedObstacles.Add(obstacle);
	}

	public void OnObstacleStoppedBeingDetected(Node2D obstacle)
	{
		_detectedObstacles.Remove(obstacle);
	}

	public void OnPlayerDetected(Node2D player)
	{
		_detectedPlayer = player;
	}

	public void OnPlayerStoppedBeingDetected(Node2D player)
	{
		_detectedPlayer = null;
		AgentManager.Instance.RemoveAgentSpottingPlayer(this);
	}

	public void AvoidObstacles()
	{
		Vector2 avoidancePosition = Vector2.Zero;
		foreach(Node2D obstacle in _detectedObstacles)
		{	
			PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;
			// use global coordinates, not local to node
			PhysicsRayQueryParameters2D query = PhysicsRayQueryParameters2D.Create(GlobalPosition, obstacle.GlobalPosition, _obstacleMask);
			Dictionary result = spaceState.IntersectRay(query);

			if(result.Count > 0)
			{
				avoidancePosition += (Vector2)result["position"] + (Vector2)result["normal"] * 50f;
			}
		}

		if(_detectedObstacles.Count > 0)
		{
			SetTargetPosition(avoidancePosition / _detectedObstacles.Count);
		}
	}

	private void SpotPlayer()
	{
		if(_detectedPlayer == null)
		{
			return;
		}

		PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;
		// use global coordinates, not local to node
		PhysicsRayQueryParameters2D query = PhysicsRayQueryParameters2D.Create(GlobalPosition, _detectedPlayer.GlobalPosition, _obstacleMask);
		Dictionary result = spaceState.IntersectRay(query);

		if(result.Count == 0)
		{
			AgentManager.Instance.PlayerPosition = _detectedPlayer.GlobalPosition;
			AgentManager.Instance.AddAgentSpottingPlayer(this);
			AgentManager.Instance.OnPlayerSpotted();
			return;
		}

		if(GlobalPosition.DistanceTo(_detectedPlayer.GlobalPosition) > GlobalPosition.DistanceTo((Vector2)result["position"]))
		{
			AgentManager.Instance.RemoveAgentSpottingPlayer(this);
			return;
		}

		AgentManager.Instance.PlayerPosition = _detectedPlayer.GlobalPosition;
		AgentManager.Instance.AddAgentSpottingPlayer(this);
		AgentManager.Instance.OnPlayerSpotted();
	}
	public void Steer(float delta)
	{
		Vector2 desieredVelocity = (_targetPosition - Position).Normalized();

		Vector2 steeringVelocity = desieredVelocity - _velocity;
		_velocity += steeringVelocity * Mathf.DegToRad(_rotationSpeed) * delta;

		LookAt(Position + _velocity.Normalized());
		Position += _velocity * delta * _maxSpeed;
	}

	public float DistanceToTarget()
	{
		return GlobalPosition.DistanceTo(_targetPosition);
	}

	public Vector2 GetVelocity()
	{
		return _velocity;
	}

	public float GetMaxSpeed()
	{
		return _maxSpeed;
	}
}
