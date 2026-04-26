using Godot;
using System;

public interface IState
{
	public void OnStateEnter();
	public void OnStateUpdate(float delta);
	public void OnStateExit();
}
