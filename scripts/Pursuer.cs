using Godot;
using System;

public partial class Pursuer : Agent
{
	[Export] private Agent _pursuitTarget;

	public override void _Process(double delta)
	{
		SetTargetPosition(_pursuitTarget.Position);
		float timeToTarget = DistanceToTarget() / GetMaxSpeed();
		Vector2 futurePosition = _targetPosition + _pursuitTarget.GetVelocity() * _pursuitTarget.GetMaxSpeed() * timeToTarget;

		SetTargetPosition(futurePosition);
		base._Process(delta);
	}
}
