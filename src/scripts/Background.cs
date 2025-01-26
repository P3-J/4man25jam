using System;
using System.Collections.Generic;
using Godot;

public partial class Background : Node2D
{
    Sprite2D BackgroundGradient;
    bool GradientIsMoving;

    public bool IsGameStartSequence;

    float GradientTargetY;

    bool IsBackgroundTransparentTransition;

    [Export]
    Parallax2D fogParallax;

    const float gameStartTransitionPixelHeight = 640f;

    const double stepsNeeded = gameStartTransitionPixelHeight / transitionStepSize;

    const float transitionStepSize = 3f;
    private bool realgamestart = false;

    int stepsDone = 0;

    [Export]
    Sprite2D backgroundGradient;

    [Export]
    Sprite2D startBackground;

    [Export]
    Parallax2D verticalBackground;

    [Export]
    Parallax2D cloudParallax;

    [Export]
    Parallax2D starsParallax;

    SignalBus sgbus;

    List<Parallax2D> transitionParallaxList;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sgbus = GetNode<SignalBus>("/root/Signalbus");
        // NEEDS TO BE CHANGED, SHOULD ONLY CALL ON FIRST LEVELUP
        sgbus.Connect("LevelUpSignal", new Callable(this, nameof(ModulateBackground)));
        sgbus.Connect("LevelUpSignal", new Callable(this, nameof(TransitionBackgroundGradient)));
        // can trigger a start message with a timer here?
    }

    public void StartGame()
    {
        IsGameStartSequence = true;
        sgbus.EmitSignal("StartGame");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (IsGameStartSequence && !realgamestart)
        {
            verticalBackground.Autoscroll = new Vector2(0, 200);
            stepsDone++;

            float startBackgroundNewY = startBackground.Position.Y + transitionStepSize;

            startBackground.Position = new Vector2(startBackground.Position.X, startBackgroundNewY);

            if (stepsDone >= stepsNeeded)
            {
                startBackground.QueueFree();
                cloudParallax.Visible = true;
                realgamestart = true;
            }
        }

        if (GradientIsMoving)
        {
            float gradientNewY = backgroundGradient.Position.Y + 3f;

            backgroundGradient.Position = new Vector2(backgroundGradient.Position.X, gradientNewY);

            GradientIsMoving = gradientNewY < GradientTargetY;
        }

        if (IsBackgroundTransparentTransition)
        {
            foreach (Parallax2D sprite in transitionParallaxList)
            {
                float newAlpha = sprite.Modulate.A - 0.01f;

                sprite.Modulate = new Color(1, 1, 1, newAlpha);

                if (newAlpha == 0f)
                {
                    sprite.QueueFree();
                    IsBackgroundTransparentTransition = false;
                }
            }
        }
    }

    public void TransitionBackgroundGradient(int nextLevel)
    {
        GradientTargetY = backgroundGradient.Position.Y + 500;
        GradientIsMoving = true;
    }

    public void ModulateBackground(int nextLevel)
    {
        switch (nextLevel)
        {
            case 1:
                transitionParallaxList = new List<Parallax2D> { verticalBackground };
                IsBackgroundTransparentTransition = true;
                break;

            case 2:

                transitionParallaxList = new List<Parallax2D> { fogParallax };
                IsBackgroundTransparentTransition = true;
                break;

            case 3:

                transitionParallaxList = new List<Parallax2D> { cloudParallax };
                IsBackgroundTransparentTransition = true;
                starsParallax.Visible = true;
                break;

            default:
                return;
        }
    }
}
