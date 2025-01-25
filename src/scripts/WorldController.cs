using System;
using System.Collections.Generic;
using Godot;

public partial class WorldController : Node2D
{
    [Export]
    PackedScene enemybase;

    [Export]
    PackedScene boss;
    Globals globals;

    public string currentlySpawning = "";
    public bool bossSpawned;
    private SignalBus sgbus;

    [Export]
    Timer spawntimer;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/Signalbus");
        GD.Randomize();
    }

    public override void _Process(double delta)
    {
        globals.AddPlayerHeight((float)(globals.playerClimbSpeed * delta));
        checkStage();
    }

    public void checkStage()
    {
        if (globals.currentLevel == 1)
        {
            spawntimer.WaitTime = 0.3f;
            if (spawntimer.IsStopped())
            {
                spawntimer.Start();
            }
            currentlySpawning = "fentplane";
            return;
        }

        if (globals.currentLevel == 2)
        {
            spawntimer.WaitTime = 2f;
            if (spawntimer.IsStopped())
            {
                spawntimer.Start();
            }
            currentlySpawning = "fentplane";
            return;
        }

        if (globals.currentLevel == 3)
        {
            spawntimer.WaitTime = 1f;
            if (spawntimer.IsStopped())
            {
                spawntimer.Start();
            }
            currentlySpawning = "fentplane";
            return;
        }

        if (globals.currentLevel == 4 && !bossSpawned)
        {
            bossSpawned = true;
            Bossscenemanager bosss = boss.Instantiate<Bossscenemanager>();
            GetTree().CurrentScene.AddChild(bosss);
            return;
        }
    }

    public void SpawnEnemy(string enemytype)
    {
        Enemybase enemy = enemybase.Instantiate<Enemybase>();

        bool spawnDirectionLeft = GD.Randi() % 2 == 1;
        // true = right , false = left
        float spawnHeight = GD.RandRange(-300, 300); // first top height,  after + bottom height = bottom + top

        if (spawnDirectionLeft)
        {
            enemy.GlobalPosition = new Vector2(1000, spawnHeight);
            enemy.enemyname = enemytype;
            GetTree().CurrentScene.AddChild(enemy);
            enemy.Fliphorizontal();
            enemy.dir = new Vector2(-1, 0);
        }
        else
        {
            enemy.GlobalPosition = new Vector2(-1000, spawnHeight);
            enemy.enemyname = enemytype;
            GetTree().CurrentScene.AddChild(enemy);
            enemy.dir = new Vector2(+1, 0);
        }
    }

    private void _on_spawntimer_timeout()
    {
        SpawnEnemy(currentlySpawning);
    }
}
