using Godot;
using System;
using System.Diagnostics;

public partial class Agent : Node2D
{
	[Export] protected bool _reverseDesieredVelocity;
	[Export] private float _maxSpeed;
	[Export] private float _rotationSpeed;
	
	protected Vector2 _targetPosition;
	private Vector2 _velocity;

    public override void _Ready()
    {
        _velocity = Vector2.Up;
    }

	public override void _Process(double delta)
	{
		Vector2 desieredVelocity;

		if (_reverseDesieredVelocity)
		{
			desieredVelocity = (Position - _targetPosition).Normalized();
		}
		else
		{
			desieredVelocity = (_targetPosition - Position).Normalized();
		}

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

		if(wrapedPosition != Position)
		{
			OnPositionWraped();
		}

		Position = wrapedPosition;
	}

	public virtual void OnPositionWraped()
	{
		
	}

	public float DistanceToTarget()
	{
		return Mathf.Sqrt((_targetPosition.X - Position.X) * (_targetPosition.X - Position.X) +
			(_targetPosition.Y - Position.Y) * (_targetPosition.Y - Position.Y));
	}

	public void SetTargetPosition(Vector2 target)
	{
		_targetPosition = target;
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
