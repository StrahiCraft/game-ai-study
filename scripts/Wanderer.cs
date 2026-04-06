using Godot;
using System;
using System.Runtime.InteropServices.Marshalling;

public partial class Wanderer : Agent
{
	[Export] private float _fov;
	[Export] private float _wanderPointDistance;
	[Export] private float _minDistanceToTarget;

    public override void _Ready()
    {
        base._Ready();
		RandomizeTargetPosition();
    }


	public override void _Process(double delta)
	{
		base._Process(delta);
		if(Mathf.Sqrt((_targetPosition.X - Position.X) * (_targetPosition.X - Position.X) +
			(_targetPosition.Y - Position.Y) * (_targetPosition.Y - Position.Y)) <= _minDistanceToTarget)
		{
			RandomizeTargetPosition();
		}
	}

	private void RandomizeTargetPosition()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();

		_targetPosition = new Vector2(Mathf.Sin(Mathf.DegToRad(rng.RandfRange(RotationDegrees - _fov / 2, RotationDegrees + _fov / 2))),
                Mathf.Cos(Mathf.DegToRad(rng.RandfRange(RotationDegrees - _fov / 2, RotationDegrees + _fov / 2)))) *
				_wanderPointDistance + Position;
	}

    public override void OnPositionWraped()
    {
        RandomizeTargetPosition();
    }
}
