using Godot;
using System;

public partial class Endscreen : Control
{
	private void _on_retry_pressed(){
		GetTree().ChangeSceneToFile("res://src/scenes/world.tscn");
	}

	private void _on_exit_pressed(){
		GetTree().Quit();
	}
}
