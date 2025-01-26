using System;
using System.Collections;
using Godot;

public partial class Enemybase : CharacterBody2D
{

	public float Speed = 300.0f;
	public Vector2 dir = new Vector2();
	public bool isAggro = false;
	public int HP = 1;

	public string enemyname = "";

	[Export]
	AnimatedSprite2D enemysprite;
	[Export]
	Node2D raycastparent;
	[Export]
	Area2D hitbox;
	[Export] PackedScene bubbleproj; 
	[Export] Timer shottimer;
	[Export] Sprite2D aimthing;
	[Export] Marker2D aimspot;
	private RayCast2D raycast;
	private Timer hittimer;
	private Globals globals;
	private SignalBus sgbus;
	private AudioStreamPlayer2D caaaw;
	private AudioStreamPlayer2D paperPlaneDeathSound;
	private AudioStreamPlayer2D jetDeathSound;
	private AudioStreamPlayer2D eagleDeathSound;
	

	private bool isPlaneFalling = false;

	private int planeFallChance = 150;
	private int planeFallCounter = 0;

	public override void _Ready()
	{
		base._Ready();
		globals = GetNode<Globals>("/root/Globals");
		raycast = raycastparent.GetNode<RayCast2D>("RayCast2D");
		sgbus = GetNode<SignalBus>("/root/Signalbus");
		hittimer = GetNode<Timer>("misc/hittimer");
		caaaw = GetNode<AudioStreamPlayer2D>("sound/caaaw");
		paperPlaneDeathSound = GetNode<AudioStreamPlayer2D>("sound/paperPlaneDeathSound");
		jetDeathSound = GetNode<AudioStreamPlayer2D>("sound/jetDeathSound");
		eagleDeathSound = GetNode<AudioStreamPlayer2D>("sound/eagleDeathSound");
		sgbus.Connect("EnemyGetHit", new Callable(this, nameof(GetHit)));


		if (enemyname == "fentplane")
		{
			Speed = 200f;
			enemysprite.Animation = "fentplane";
		}

		if (enemyname == "eagle"){
			Speed = 200f;
			enemysprite.Animation = "eagle";
		}


        if (enemyname == "jet"){
            Speed = 300f;
            Scale = new Vector2(1.25f, 1.25f);
            enemysprite.Animation = "jet";
        }


		enemysprite.Play();
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (enemyname == "eagle")
		{
			raycastparent.LookAt(globals.player.GlobalPosition);
			raycastparent.Rotation += Mathf.Pi / 2;
			if (raycast.IsColliding() && !isAggro)
			{
				AudioStreamPlayer2D caaaw = GetNode<AudioStreamPlayer2D>("sound/caaaw");
				caaaw.Play();

				CharacterBody2D collider = (CharacterBody2D)raycast.GetCollider();
				if (!collider.IsInGroup("player"))
				{
					return;
				}
				Speed = 250;
				enemysprite.LookAt(globals.player.GlobalPosition);

			   
				enemysprite.FlipV = globals.player.GlobalPosition.X < GlobalPosition.X;
			
				isAggro = true;
				dir = (globals.player.GlobalPosition - GlobalPosition).Normalized();
			}
		}

		if (enemyname == "fentplane")
		{
			if (!isPlaneFalling)
			{
				isPlaneFalling = GD.Randi() % planeFallChance == 0;
			}
			else
			{
				if (planeFallCounter < 1)
				{
					var leftOrRight = (GD.Randi() % 2 == 0) ? 1 : -1;
					dir = new Vector2(dir.X * leftOrRight, 0.66f);
					planeFallCounter = 30;
				}
				planeFallCounter--;
			}
		}

		if (enemyname == "jet"){

			raycastparent.Rotation += 0.1f;

			if (shottimer.IsStopped()){
				shottimer.Start();
			}
		}

		CheckIfOutOfBounds();

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

	private void _on_shottimer_timeout(){
		CreateBubble(aimspot.GlobalPosition);
	}

	private void GetHit(Node2D body, int amount, Area2D bubbleproj)
	{
		if ((Area2D)body != hitbox)
		{
			return;
		}
		enemysprite.Material.Set("shader_parameter/active", true);
		HP -= amount;

		CheckHp();

		hittimer.Start();
		bubbleproj.QueueFree();
	}

	public void CheckHp()
	{
		if (HP <= 0)
		{
			if (enemyname == "fentplane")
			{
				paperPlaneDeathSound.Play();
			}
			if (enemyname == "jet")
			{
				jetDeathSound.Play();
			}
			if (enemyname == "eagle")
			{
				eagleDeathSound.Play();
			}
			QueueFree();
		}
	}

	private void _on_hittimer_timeout()
	{
		enemysprite.Material.Set("shader_parameter/active", false);
	}


	private void CreateBubble(Vector2 AimPos)
	{
		Bubbleprojectile crntBubble = (Bubbleprojectile)bubbleproj.Instantiate();
		crntBubble.ZIndex = 0;
		crntBubble.enemyowner = true;
		crntBubble.dir = (AimPos - Position).Normalized();
		crntBubble.speed = 300f;
		crntBubble.Scale = new Vector2(0.75f, 0.75f); 
		GetTree().CurrentScene.AddChild(crntBubble);
		crntBubble.GlobalPosition = GlobalPosition;
	}


	private void CheckIfOutOfBounds()
	{
		if (
			GlobalPosition.X > 2000
			|| GlobalPosition.X < -2000
			|| GlobalPosition.Y > 2000
			|| GlobalPosition.Y < -2000
		)
		{
			QueueFree();
		}
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			sgbus.EmitSignal("PlayerGetHit", 1);
		}
	}

	public void Fliphorizontal()
	{
		enemysprite.FlipH = true;
	}

}
