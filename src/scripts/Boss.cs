using Godot;
using System;

public partial class Boss : Area2D
{
	public bool started = false;
	public void FinishedEntry(){
		started = true;
	}
}
