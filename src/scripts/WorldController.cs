using System;
using Godot;

public partial class WorldController : Node2D
{
    [Export]
    PackedScene enemybase;

    Globals globals;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
    }

    public override void _Process(double delta)
    {
        globals.AddPlayerHeight((float)(globals.playerClimbSpeed * delta));
    }

    public void SpawnEnemy(string enemytype)
    {
        GD.Randomize();

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
        SpawnEnemy("test");
    }
}
