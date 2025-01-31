using Godot;
using System;

public partial class Bubbleprojectile : Area2D
{
	public Vector2 dir = new Vector2();
	public float speed = 300;
	public int damage = 1;
	public bool canMove = false;
	public bool canScale = false;

	public bool enemyowner = false;
	
	private SignalBus sgbus;

	public override void _Ready()
	{
		base._Ready();
		sgbus = GetNode<SignalBus>("/root/Signalbus");
	}

	public override void _Process(double delta)
	{
		if (!canMove && canScale) {
			if (Scale.X < 2f){
				Vector2 newscale = Scale;
				newscale.X += 0.03f;
				newscale.Y += 0.03f;
				Scale = newscale;
			}
		}
		GlobalPosition += dir.Normalized() * (float)(speed * delta);
	}

	private void _on_area_entered(Node2D body){

		if (enemyowner){
			return;
		}

		if (body.IsInGroup("enemy") && canMove){
			sgbus.EmitSignal("EnemyGetHit", body , damage, this);
			return;
		}

		if (body.IsInGroup("boss") && canMove){
			sgbus.EmitSignal("BossHit", damage);
			QueueFree();
			return;
		}

		if (body.IsInGroup("bubble") && canMove){
			body.QueueFree();
			QueueFree();
		}

	}

	private void _on_body_entered(Node2D body){
		if (!enemyowner){return;}

		if (body.IsInGroup("player")){
			sgbus.EmitSignal("PlayerGetHit", 1);
			QueueFree();
		}
	}

}
