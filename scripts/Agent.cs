using Godot;
using System;
using System.Diagnostics;

public partial class Agent : Node2D
{
	[Export] private float _maxSpeed;
	[Export] private float _rotationSpeed;
	
	private Vector2 _targetPosition;
	private Vector2 _velocity;

    public override void _Ready()
    {
        _velocity = Vector2.Up;
    }

	public override void _Process(double delta)
	{
		Vector2 desieredVelocity = (_targetPosition - Position).Normalized();
		Vector2 steeringVelocity = desieredVelocity - _velocity;

		_velocity += steeringVelocity * Mathf.DegToRad(_rotationSpeed) * (float)delta;

		LookAt(Position + _velocity.Normalized());
		Position += _velocity * (float)delta * _maxSpeed;
		WrapPosition();
	}

	private void WrapPosition()
	{
		Vector2 wrapedPosition = Position;

		if(Position.X < -GetWindow().Size.X / 2)
		{
			wrapedPosition.X = GetWindow().Size.X / 2;
		}
		if(Position.Y < -GetWindow().Size.Y / 2)
		{
			wrapedPosition.Y = GetWindow().Size.Y / 2;
		}

		if(Position.X > GetWindow().Size.X / 2)
		{
			wrapedPosition.X = -GetWindow().Size.X / 2;
		}
		if(Position.Y > GetWindow().Size.Y / 2)
		{
			wrapedPosition.Y = -GetWindow().Size.Y / 2;
		}

		Position = wrapedPosition;
	}

	public void SetTargetPosition(Vector2 target)
	{
		_targetPosition = target;
	}

	public Vector2 GetVelocity()
	{
		return _velocity;
	}
}
