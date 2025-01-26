using System;
using Godot;

public partial class playahh : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private SignalBus sgbus;
	private Globals globals;

	public override void _Ready()
	{
		base._Ready();
		sgbus = GetNode<SignalBus>("/root/Signalbus");
		globals = GetNode<Globals>("/root/Globals");

		sgbus.Connect("ExampleSignal", new Callable(this, nameof(RaiseCoins)));
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void RaiseCoins(int amount)
	{
		globals.exampleGlobal += amount;
		globals.GlobalPrint();
	}
}
