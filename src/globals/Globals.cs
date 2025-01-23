using Godot;

public partial class Globals : Node
{

    public int exampleGlobal = 0;

    public void GlobalPrint(){
        GD.Print(exampleGlobal);
    }

}