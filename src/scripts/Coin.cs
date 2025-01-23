using Godot;
using System;

public partial class Coin : Area2D
{

	private SignalBus sgbus;

	public override void _Ready()
	{
		sgbus = GetNode<SignalBus>("/root/Signalbus");
	}

	private void _on_body_entered(Node2D body){

		if (body.IsInGroup("player")){
			sgbus.EmitSignal("ExampleSignal", 1);
		}
	}
}
