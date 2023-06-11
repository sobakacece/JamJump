using Godot;
using System;

public class HUD : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] Label scoreLabel;
    Camera2D camera;
    float currentScore = 0;
    float highestScore = 0;
    ScreenManager screenManager;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera = GetNode<Camera2D>("/root/TestLevel/Camera2D");
        scoreLabel = GetNode<Label>("VBoxContainer/Label");
        screenManager = GetNode<ScreenManager>("/root/ScreenManager");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        currentScore = -camera.GlobalPosition.y;
        if (currentScore > highestScore)
            highestScore = currentScore;

        scoreLabel.Text = ((highestScore).ToString("00000000"));
        screenManager.MyFinalScore = highestScore;
    }
}
