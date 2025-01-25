using Godot;
using System;

public partial class Enemybase : CharacterBody2D
{
	public const float Speed = 300.0f;
	public Vector2 dir = new Vector2();
	public bool isAggro = false;
	[Export] AnimatedSprite2D enemysprite;
	[Export] Node2D raycastparent;
	private RayCast2D raycast;
		
	private Globals globals;
    public override void _Ready()
    {
        base._Ready();
		globals = GetNode<Globals>("/root/Globals");
		raycast = raycastparent.GetNode<RayCast2D>("RayCast2D");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		raycastparent.LookAt(globals.player.GlobalPosition);
		raycastparent.Rotation += Mathf.Pi / 2;

		if (raycast.IsColliding() && !isAggro){
			
			CharacterBody2D collider = (CharacterBody2D)raycast.GetCollider();

			if (!collider.IsInGroup("player")) {return;}
			isAggro = true;
			dir = (globals.player.GlobalPosition - GlobalPosition).Normalized();
			GD.Print(dir);

		}

		Vector2 direction = dir;
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void Fliphorizontal(){
		enemysprite.FlipH = true;
	}
}
