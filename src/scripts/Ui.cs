using System;
using Godot;

public partial class Ui : Node2D
{

	[Export]
	RichTextLabel startHeight;

	[Export]
	RichTextLabel endHeight;

	[Export]
	ProgressBar heightProgressBar;

	Globals globals;
	private SignalBus sgbus;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		globals = GetNode<Globals>("/root/Globals");
		sgbus = GetNode<SignalBus>("/root/Signalbus");
		sgbus.Connect("LevelUpSignal", new Callable(this, nameof(ProgressBarNextLevel)));

		int nextLevelHeightNeeded = globals.LevelHeightNeededArr[0];
		endHeight.Text = $"{nextLevelHeightNeeded}m";
		heightProgressBar.MaxValue = nextLevelHeightNeeded;
		heightProgressBar.MinValue = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		startHeight.Text = $"{Math.Floor(globals.playerHeight)}m";
		heightProgressBar.Value = globals.playerHeight;
	}

	public void ProgressBarNextLevel(int nextLevel)
	{
		int maxLevel = globals.LevelHeightNeededArr.Count - 1;

		if (maxLevel >= nextLevel)
		{
			int currentLevelHeight = globals.LevelHeightNeededArr[nextLevel - 1];
			heightProgressBar.MinValue = currentLevelHeight;

			int nextLevelHeightNeeded = globals.LevelHeightNeededArr[nextLevel];
			endHeight.Text = $"{nextLevelHeightNeeded}m";
			heightProgressBar.MaxValue = nextLevelHeightNeeded;
		}
	}

}
