using Godot;
using System;

public class SpikePlatform : Platform
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Area2D hurtBox = GetNode<Area2D>("Area2D");
        hurtBox.Connect("body_entered", this, "OnCollision");
    }
    public void OnCollision(Node2D body)
    {
        if (body is Player)
        {
            Player player = (Player)body;
            player.Death();
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
