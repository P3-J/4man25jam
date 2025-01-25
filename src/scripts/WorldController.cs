using System;
using System.Collections.Generic;
using Godot;

public partial class WorldController : Node2D
{
    [Export]
    PackedScene enemybase;

    Globals globals;

    private SignalBus sgbus;

    readonly List<string> LevelEnemyList = new() { "eagle", "eagle", "eagle", "eagle", "eagle" };
    readonly List<float> LevelEnemySpawnTime = new() { 3f, 2f, 1f, 1f, 1f };

    [Export]
    Timer spawntimer;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/Signalbus");
        GD.Randomize();

        spawntimer.WaitTime = LevelEnemySpawnTime[globals.currentLevel];

        sgbus.Connect("LevelUpSignal", new Callable(this, nameof(SetNewSpawnTime)));
    }

    public void SetNewSpawnTime(int nextLevel)
    {
        spawntimer.WaitTime = LevelEnemySpawnTime[nextLevel];
    }

    public override void _Process(double delta)
    {
        globals.AddPlayerHeight((float)(globals.playerClimbSpeed * delta));
    }

    public void SpawnEnemy(string enemytype)
    {
        Enemybase enemy = enemybase.Instantiate<Enemybase>();

        bool spawnDirectionLeft = GD.Randi() % 2 == 1;
        // true = right , false = left
        float spawnHeight = GD.Randi() % 300 + 100; // first top height,  after + bottom height = bottom + top

        if (spawnDirectionLeft)
        {
            enemy.GlobalPosition = new Vector2(1000, -spawnHeight);
            GetTree().CurrentScene.AddChild(enemy);
            enemy.Fliphorizontal();
            enemy.dir = new Vector2(-1, 0);
        }
        else
        {
            enemy.GlobalPosition = new Vector2(-1000, -spawnHeight);
            GetTree().CurrentScene.AddChild(enemy);
            enemy.dir = new Vector2(+1, 0);
        }
    }

    private void _on_spawntimer_timeout()
    {
        //SpawnEnemy(LevelEnemyList[globals.currentLevel]);
    }

}
