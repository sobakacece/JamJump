using Godot;
using System;

public class GameOver : GameScreen
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Label scoreLabel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("HBoxContainer/Score");
        base._Ready();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
       
    }

}
