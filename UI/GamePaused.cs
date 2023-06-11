using Godot;
using System;

public class GamePaused : GameScreen
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] Button btnContinue;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        btnContinue = GetNode<Button>("HBoxContainer/Continue");
        btnContinue.Connect("pressed", this, "Continue");
    }
    public void Continue()
    {
        GetTree().Paused = false;
        this.Visible = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
