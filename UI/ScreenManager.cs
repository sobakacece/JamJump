using Godot;
using System;

public class ScreenManager : Node
{
    public GameScreen gamePaused;
    public override void _Ready()
    {
        gamePaused = AddGlobalScreen("res://UI/GamePaused.tscn");
    }
    public GameScreen AddGlobalScreen(string path)
    {
        GameScreen MyScreen = new GameScreen();
        PackedScene Screen = (PackedScene)ResourceLoader.Load(path);
        MyScreen = (GameScreen)Screen.Instance();
        this.AddChild(MyScreen);
        MyScreen.Visible = false;
        return MyScreen;
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
