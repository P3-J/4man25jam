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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Globals globals = GetNode<Globals>("/root/Globals");
        startHeight.Text = $"{globals.playerHeight}m";

        int nextLevelHeightNeeded = globals.LevelHeightNeededArr[globals.currentLevel + 1];
        int currentLevelHeight = globals.LevelHeightNeededArr[globals.currentLevel];
        endHeight.Text = $"{nextLevelHeightNeeded}m";

        heightProgressBar.MinValue = currentLevelHeight;
        heightProgressBar.MaxValue = nextLevelHeightNeeded;
        heightProgressBar.Value = globals.playerHeight;
    }
}
