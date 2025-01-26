using Godot;
using System;

public partial class Bossscenemanager : Node2D
{
	[Export] Boss boss;
	[Export] ProgressBar bosshpbar;
	[Export] AnimationPlayer animplayer;

	[Export] Marker2D spot1;
	[Export] Marker2D spot2;
	[Export] Marker2D spot3;
	[Export] Marker2D spot4;
	[Export] Marker2D spot5;
	[Export] Marker2D startspot;
	[Export] PackedScene bubbleproj;
	[Export] Timer phase2timer;

	private AnimatedSprite2D bosssprite;
	public int bosshp = 10;
	public SignalBus sgbus;
	[Export] Timer hittimer;

	private int currentphase;
	public bool phasing;
	public override void _Ready()
	{
		sgbus = GetNode<SignalBus>("/root/Signalbus");
		bosshpbar.Value = bosshp;
		sgbus.Connect("BossHit", new Callable(this, nameof(GetHit)));
		bosssprite = boss.GetNode<AnimatedSprite2D>("bosssprite");
	}


	public override void _Process(double delta)
	{
		base._Process(delta);

		if (!boss.started){
			return;
		}

		if (!phasing){
			DoPhase();
		}
		

	}

	private void DoPhase()
	{
		phasing = true;
		if (currentphase == 0){
			animplayer.Play("new_animation");
			return;
		}

		if (currentphase == 1){

			CreateBubble(spot1);
			CreateBubble(spot3);
			CreateBubble(spot5);
			phase2timer.Start();
			return;
		}

		if (currentphase == 2){

			CreateBubble(spot1);
			CreateBubble(spot5);
			phase2timer.Start();
			return;
		}

		if (currentphase == 3){
			
			sgbus.EmitSignal("SpawnEnemy", "eagle");
			sgbus.EmitSignal("SpawnEnemy", "fentplane");
			sgbus.EmitSignal("SpawnEnemy", "jet");
			
			phasing = false;
			currentphase = 0;
		}
	}

	public void NotPhasing(){
		phasing = false;
		currentphase += 1;
	}

	private void _on_phase_2_timer_timeout(){
		phasing = false;
		currentphase += 1;
	}


	private void CreateBubble(Marker2D endMarker)
	{
		Bubbleprojectile crntBubble = (Bubbleprojectile)bubbleproj.Instantiate();
		crntBubble.ZIndex = 0;
		crntBubble.enemyowner = true;
		crntBubble.canScale = true;
		crntBubble.dir = (endMarker.GlobalPosition - startspot.GlobalPosition).Normalized();
		GetTree().CurrentScene.AddChild(crntBubble);
		crntBubble.GlobalPosition = startspot.GlobalPosition;
	}


	public void GetHit(int damage){
		bosshp -= damage;
		bosshpbar.Value = bosshp;
		hittimer.Start();
		bosssprite.Material.Set("shader_parameter/active", true);
		CheckForDeath();
	}

	private void _on_hittimer_timeout(){

		bosssprite.Material.Set("shader_parameter/active", false);
	}

	public void CheckForDeath(){
		if (bosshp <= 0){
			boss.started = false;
			animplayer.Play("death");
		}
	}

	public void SwitchToEnd(){
		GetTree().ChangeSceneToFile("res://src/scenes/endscreen.tscn");
	}

	public void _on_laser_body_entered(Node2D body){

		if (body.IsInGroup("player")){
			sgbus.EmitSignal("PlayerGetHit", 1);
		}
	}

	
}
