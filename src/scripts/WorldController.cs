using Godot;
using System;

public partial class WorldController : Node2D
{
	[Export] Background bg;
	public override void _Ready()
	{
		bg.TransitionBackgroundGradient(1200f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
