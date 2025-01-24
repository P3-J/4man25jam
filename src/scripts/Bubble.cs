using Godot;
using System;

public partial class Bubble : CharacterBody2D
{
	public float BoundsFromCenterOfScreenX = 200f;
	public float BoundsFromCenterOfScreenY = 250f;
	[Export] public float MaxSpeed = 200f; 
    [Export] public float Acceleration = 400f; 
    [Export] public float Deceleration = 500f;
	[Export] public Sprite2D bubblesprite;
	private bool swaying = false;
	private string swayingDir = "";

	public override void _PhysicsProcess(double delta)
	{
        Vector2 velocity = Velocity;

        Vector2 direction = PlayerInput(); 

		if (!swaying){
			bubblesprite.Rotation = Mathf.MoveToward(bubblesprite.Rotation, 0, 0.01f);
		} else {
			if (swayingDir == "right"){
				bubblesprite.Rotation = Mathf.MoveToward(bubblesprite.Rotation, 10, 0.007f);
			} else {
				bubblesprite.Rotation = Mathf.MoveToward(bubblesprite.Rotation, -10, 0.007f);
			}
		}

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

		return Direction.Normalized();

	}


	private bool CheckIfInBounds(string dir, bool negative){ 
		float bfcosX = BoundsFromCenterOfScreenX;
		float bfcosY = BoundsFromCenterOfScreenY;

		if (dir == "vertical"){
			
			if (negative){
				return Position.Y > -bfcosY;
			}
			return Position.Y < bfcosY;
		}

		if (dir == "horizontal"){
			
			if (negative){
				return Position.X > -bfcosX;
			}
			return Position.X < bfcosX;
		}
		return false;
	}


}
