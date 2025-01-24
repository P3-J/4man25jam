using Godot;
using System;

public partial class Bubbleprojectile : Area2D
{
	public Vector2 dir = new Vector2();
	public float speed = 150;
	public bool canMove = false;
	public override void _Process(double delta)
	{
		if (!canMove) {
			if (Scale.X < 2f){
				Vector2 newscale = Scale;
				newscale.X += 0.03f;
				newscale.Y += 0.03f;
				Scale = newscale;
			}
		}
		GlobalPosition += dir.Normalized() * (float)(speed * delta);
	}
}
