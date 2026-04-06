using Godot;
using System;

public partial class Evader : Wanderer
{
	[Export] private Agent _evadeTarget;
	[Export] private float _evasionDistance;
	[Export] private float _minimumEvasionTime;

	private float _evasionTimer = 0;

	public override void _Process(double delta)
	{
		if(_evasionTimer > 0)
		{
			_evasionTimer -= (float)delta;
		}
		if(DistanceToTarget() <= _evasionDistance || _evasionTimer > 0)
		{	
			_reverseDesieredVelocity = true;
			SetTargetPosition(_evadeTarget.Position);
			float timeToTarget = DistanceToTarget() / GetMaxSpeed();
			Vector2 futurePosition = _targetPosition + _evadeTarget.GetVelocity() * _evadeTarget.GetMaxSpeed() * timeToTarget;
			_evasionTimer = _minimumEvasionTime;

			SetTargetPosition(futurePosition);
		}
		else
		{
			_reverseDesieredVelocity = false;
			RandomizeTargetPosition();
		}
		base._Process(delta);
	}

    public override void OnPositionWraped()
    {
        _evasionTimer = 0;
    }
}
