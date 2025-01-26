using Godot;
using System;

public partial class Failurescreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private void _on_retry_pressed(){
		GetTree().ChangeSceneToFile("res://src/scenes/world.tscn");
	}

	private void _on_exit_pressed(){
		GetTree().Quit();
	}
}
