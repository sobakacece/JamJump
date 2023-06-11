using Godot;
using System;

public class Breakable : Platform
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    AnimationPlayer animationPlayer;
    Timer timer;
    [Export] string breakAnimation = "break";
    public override void _Ready()
    {
        base._Ready();
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        timer = GetNode<Timer>("Timer");
        this.Connect("body_entered", this, "OnCollision");
        timer.Connect("timeout", this, "Despawn");
    }
    public void OnCollision(Node2D body)
    {
        if (body is Player && body.GlobalPosition.y < this.GlobalPosition.y - 30)
        {
            timer.Start();
            animationPlayer.Play(breakAnimation);

        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
