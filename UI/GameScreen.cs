using Godot;
using System;

public class GameScreen : CanvasLayer
{
    PackedScene testLevelScene;
    [Export] public Button btnQuit, btnRestart;
    public override void _Ready()
    {
         testLevelScene = (PackedScene)ResourceLoader.Load("res://TestLevel.tscn");
        btnQuit = GetNode<Button>("HBoxContainer/Quit");
        btnRestart = GetNode<Button>("HBoxContainer/Restart");
        ConnectToNodes();
    }
    public void ConnectToNodes()
    {
        btnQuit.Connect("pressed", this, "Quit");
        btnRestart.Connect("pressed", this, "Restart");
    }
    public void Quit()
    {
        GetTree().Quit();
    }
    public void Restart()
    {
        var root = GetTree().Root;
        root.RemoveChild(root.GetNode<TestLevel>("TestLevel"));
        
        GetTree().Root.AddChild(testLevelScene.Instance());
        GetTree().Paused = false;
        this.Visible = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
