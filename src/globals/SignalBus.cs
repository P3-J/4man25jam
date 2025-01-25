using System;
using Godot;

public partial class SignalBus : Node
{
    [Signal]
    public delegate void ExampleSignalEventHandler();

    [Signal]
    public delegate void LevelUpSignalEventHandler(int nextLevel);
}
