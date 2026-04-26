using Godot;
using System;
using System.Collections.Generic;

public partial class AgentManager : Node
{
	public Vector2 PlayerPosition { get; set; }
	public bool PlayerSpotted { get => _agentsSpottingPlayer.Count > 0; }

	private List<Agent> _agents = new List<Agent>();
	public HashSet<Agent> _agentsSpottingPlayer = new HashSet<Agent>();


	public static AgentManager Instance;

    public override void _EnterTree()
    {
        if(Instance == null)
		{
			Instance = this;
			return;
		}
		QueueFree();
    }

	public void AddAgent(Agent agent)
	{
		_agents.Add(agent);
	}

	public void AddAgentSpottingPlayer(Agent agent)
	{
		_agentsSpottingPlayer.Add(agent);
	}

	public void RemoveAgentSpottingPlayer(Agent agent)
	{
		_agentsSpottingPlayer.Remove(agent);
	}

	public void OnPlayerSpotted()
	{
		foreach(Agent agent in _agents)
		{
			agent.OnPlayerSpotted();
		}
	}
}
