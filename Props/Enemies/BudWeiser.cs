using Godot;
using System;

public class BudWeiser : Enemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 direction;
    float speed;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Timer timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, "Jump");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        this.Position += Vector2.Left * speed * delta;
    }
}