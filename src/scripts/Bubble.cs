using Godot;
using System;

public partial class Bubble : CharacterBody2D
{
	[Export] public float MaxSpeed = 200f; 
    [Export] public float Acceleration = 400f; 
    [Export] public float Deceleration = 500f;
	[Export] public Sprite2D bubblesprite;
	[Export] private Node2D aimrotater;
	[Export] PackedScene bubbleproj;
	private Marker2D shootStartPoint;
	private Marker2D shootEndPoint;

	public float BoundsFromCenterOfScreenX = 200f;
	public float BoundsFromCenterOfScreenY = 250f;
	private Bubbleprojectile currentProj;
	private bool swaying = false;
	private string swayingDir = "";
	private bool chargingBubbleGun = false;


    public override void _Ready()
    {
        base._Ready();
		shootStartPoint = aimrotater.GetNode<Marker2D>("startchargespot");
		shootEndPoint = aimrotater.GetNode<Marker2D>("aimtowards");
    }

    public override void _PhysicsProcess(double delta)
	{
        Vector2 velocity = Velocity;

        Vector2 direction = PlayerInput(); 

		Sway();

        if (direction != Vector2.Zero)
        {
            velocity.X = Mathf.MoveToward(velocity.X, direction.X * MaxSpeed, Acceleration * (float)delta);
            velocity.Y = Mathf.MoveToward(velocity.Y, direction.Y * MaxSpeed, Acceleration * (float)delta);
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, Deceleration * (float)delta);
            velocity.Y = Mathf.MoveToward(velocity.Y, 0, Deceleration * (float)delta);
        }

        Velocity = velocity;
        MoveAndSlide();
    }


	private void Sway(){
		if (!swaying){
			bubblesprite.Rotation = Mathf.MoveToward(bubblesprite.Rotation, 0, 0.01f);
		} else {
			if (swayingDir == "right"){
				bubblesprite.Rotation = Mathf.MoveToward(bubblesprite.Rotation, 10, 0.007f);
			} else {
				bubblesprite.Rotation = Mathf.MoveToward(bubblesprite.Rotation, -10, 0.007f);
			}
		}
	}

	private void ShootOrChargeBubble(bool bubbleState){

		if (bubbleState){
			chargingBubbleGun = true;
			CreateBubble();
		} else {
			chargingBubbleGun = false;
			shootStartPoint.RemoveChild(currentProj);
			GetTree().CurrentScene.AddChild(currentProj);
			currentProj.GlobalPosition = shootStartPoint.GlobalPosition;
			currentProj.dir = shootEndPoint.GlobalPosition - shootStartPoint.GlobalPosition;
			currentProj.canMove = true;
		}

	}

    private void CreateBubble()
    {
		currentProj = null;
        Bubbleprojectile crntBubble = (Bubbleprojectile)bubbleproj.Instantiate();
		currentProj = crntBubble;
		shootStartPoint.AddChild(crntBubble);
		crntBubble.ZIndex = -1;
		crntBubble.GlobalPosition = shootStartPoint.GlobalPosition;
    }

    private Vector2 PlayerInput(){
		Vector2 Direction = new Vector2();
		swaying = false;

		if (Input.IsActionPressed("up") && CheckIfInBounds("vertical", true)){
			Direction.Y = -1;
		}

		if (Input.IsActionPressed("down") && CheckIfInBounds("vertical", false)){
			Direction.Y = 1;
		}
		
		if (Input.IsActionPressed("left") && CheckIfInBounds("horizontal", true)){
			Direction.X = -1;
			swayingDir = "left";
			swaying = true;
		}
		
		if (Input.IsActionPressed("right") && CheckIfInBounds("horizontal", false)){
			Direction.X = 1;
			swayingDir = "right";
			swaying = true;
		}


		if (Input.IsActionPressed("aimup")){
			aimrotater.Rotation += 0.05f;
		}

		if (Input.IsActionPressed("aimdown")){
			aimrotater.Rotation -= 0.05f;
		}

		if (Input.IsActionJustPressed("charge") && !chargingBubbleGun){
			ShootOrChargeBubble(true);
		}
		if (Input.IsActionJustReleased("charge") && chargingBubbleGun){
			ShootOrChargeBubble(false);
		}

		return Direction.Normalized();

	}


	private bool CheckIfInBounds(string dir, bool negative){ 
		float bfcosX = BoundsFromCenterOfScreenX;
		float bfcosY = BoundsFromCenterOfScreenY;

		if (dir == "vertical"){
			
			if (negative){
				return GlobalPosition.Y > -bfcosY;
			}
			return GlobalPosition.Y < bfcosY;
		}

		if (dir == "horizontal"){
			
			if (negative){
				return GlobalPosition.X > -bfcosX;
			}
			return GlobalPosition.X < bfcosX;
		}
		return false;
	}


}
