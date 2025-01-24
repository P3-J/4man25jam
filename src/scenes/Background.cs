using System;
using Godot;

public partial class Background : Node2D
{
    Sprite2D BackgroundGradient;
    bool GradientIsMoving;

    float GradientTargetY;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BackgroundGradient = GetNode<Sprite2D>("BackgroundGradient");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (GradientIsMoving)
        {
            float gradientNewY = BackgroundGradient.Position.Y + 3f;

            BackgroundGradient.Position = new Vector2(BackgroundGradient.Position.X, gradientNewY);

            GradientIsMoving = gradientNewY < GradientTargetY;
        }
    }

    public void TransitionBackgroundGradient(float pixelChange)
    {
        GradientTargetY = BackgroundGradient.Position.Y + pixelChange;
        GradientIsMoving = true;
    }
}
