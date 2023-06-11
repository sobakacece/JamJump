using Godot;
using System;

public class MovingPlatform : Platform
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] float speed = 100f;
    float spawnPoint;
    [Export] float range = 400;
    Vector2 direction = Vector2.Right;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        spawnPoint = this.Position.x;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        this.Position += direction * speed * delta;
        if (Mathf.Abs(this.Position.x) > range)
        {
            direction = -direction;
        }
        base._Process(delta);
    }
}
