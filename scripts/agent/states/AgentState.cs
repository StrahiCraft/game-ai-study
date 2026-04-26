using Godot;
using System;

public abstract partial class AgentState : Node, IState
{
	[Export] protected Agent _agentStateManager;

    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void OnStateUpdate(float delta);
}
