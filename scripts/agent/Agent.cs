using Godot;

public partial class Agent : StateManager
{
	[Export] private float _maxSpeed;
	[Export] private float _rotationSpeed;
	
	protected Vector2 _targetPosition;
	private Vector2 _velocity;

    public override void _Ready()
    {
        AgentManager.Instance.AddAgent(this);
		base._Ready();
    }

	public void SetTargetPosition(Vector2 targetPosition)
	{
		_targetPosition = targetPosition;
	}

	public void OnPlayerSpotted()
	{
		// TODO change to going to last player position state
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
		return Mathf.Sqrt((_targetPosition.X - Position.X) * (_targetPosition.X - Position.X) +
			(_targetPosition.Y - Position.Y) * (_targetPosition.Y - Position.Y));
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
