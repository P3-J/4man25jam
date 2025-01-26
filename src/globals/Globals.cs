using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;

public partial class Globals : Node
{
	private SignalBus sgbus;
	public int exampleGlobal = 0;

	public float playerHeight = 0;

	public int currentLevel = 0;

	public float playerClimbSpeed = 3f;

	public Bubble player;

    public readonly List<int> LevelHeightNeededArr = new() { 30, 60, 120 };

	public void GlobalPrint()
	{
		GD.Print(exampleGlobal);
	}

	public override void _Ready()
	{
		sgbus = GetNode<SignalBus>("/root/Signalbus");
	}

	public void AddPlayerHeight(float height)
	{
		playerHeight += height;

		// win game
		int maxLevel = LevelHeightNeededArr.Count - 1;
		if (currentLevel == maxLevel)
		{
			return;
		}

		// check if height hit new level
		int nextLevelHeightNeeded = LevelHeightNeededArr[currentLevel];
		if (playerHeight >= nextLevelHeightNeeded)
		{
			int nextLevel = currentLevel + 1;
			currentLevel = nextLevel;
			sgbus.EmitSignal("LevelUpSignal", nextLevel);
		}
		// if yes then signalbus event new level
	}
}
