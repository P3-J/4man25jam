using System;
using Godot;

public partial class Background : Node2D
{
    Sprite2D BackgroundGradient;
    bool GradientIsMoving;

    bool IsGameStartSequence;

    float GradientTargetY;

    const float gameStartTransitionPixelHeight = 640f;

    const double stepsNeeded = gameStartTransitionPixelHeight / transitionStepSize;

    const float transitionStepSize = 3f;

    int stepsDone = 0;

    [Export]
    Sprite2D backgroundGradient;

    [Export]
    Sprite2D startBackground;

    [Export]
    Parallax2D verticalBackground;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // can trigger a start message with a timer here?
        IsGameStartSequence = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (IsGameStartSequence)
        {
            stepsDone++;

            float startBackgroundNewY = startBackground.Position.Y + transitionStepSize;

            startBackground.Position = new Vector2(startBackground.Position.X, startBackgroundNewY);

            if (stepsDone >= stepsNeeded)
            {
                startBackground.QueueFree();
                IsGameStartSequence = false;
            }
        }

        if (GradientIsMoving)
        {
            float gradientNewY = backgroundGradient.Position.Y + 3f;

            backgroundGradient.Position = new Vector2(backgroundGradient.Position.X, gradientNewY);

            GradientIsMoving = gradientNewY < GradientTargetY;
        }
    }

    public void TransitionBackgroundGradient(float pixelChange)
    {
        GradientTargetY = backgroundGradient.Position.Y + pixelChange;
        GradientIsMoving = true;
    }
}
