using Godot;
using System;

public partial class Startscreen : Node2D
{
	

	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://src/scenes/world.tscn");
	}

}
