using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[ExportCategory("Stats")]
	[Export] protected float _movementSpeed = 300f;

	protected Vector2 _velocity;

	public override void _Process(double delta)
	{
		HandleInput();
		Move();
	}

	protected virtual void HandleInput()
	{
		_velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
		{
			_velocity += Vector2.Up;
		}
		if (Input.IsActionPressed("move_down"))
		{
			_velocity += Vector2.Down;
		}
		if (Input.IsActionPressed("move_left"))
		{
			_velocity += Vector2.Left;
		}
		if (Input.IsActionPressed("move_right"))
		{
			_velocity += Vector2.Right;
		}

		_velocity = _velocity.Normalized() * _movementSpeed;
	}

	protected void Move()
	{
		UpDirection = Vector2.Up;
		Velocity = _velocity;
		MoveAndSlide();
	}
}
