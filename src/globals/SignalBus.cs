using System;
using Godot;

public partial class SignalBus : Node
{
    [Signal]
    public delegate void ExampleSignalEventHandler();
    [Signal]
    public delegate void PlayerGetHitEventHandler();
    [Signal]
    public delegate void EnemyGetHitEventHandler();
    [Signal]
    public delegate void LevelUpSignalEventHandler(int nextLevel);
    [Signal]
    public delegate void BossHitEventHandler();
    [Signal]
    public delegate void SpawnEnemyEventHandler();
    
    [Signal]
    public delegate void StartGameEventHandler();

}
