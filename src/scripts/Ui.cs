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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        startHeight.Text = $"{Math.Floor(globals.playerHeight)}m";
        if (!globals.isPlayerWin)
        {
            int nextLevelHeightNeeded = globals.LevelHeightNeededArr[globals.currentLevel + 1];
            endHeight.Text = $"{nextLevelHeightNeeded}m";
            heightProgressBar.MaxValue = nextLevelHeightNeeded;
        }
        int currentLevelHeight = globals.LevelHeightNeededArr[globals.currentLevel];

        heightProgressBar.MinValue = currentLevelHeight;
        heightProgressBar.Value = globals.playerHeight;
    }
}
