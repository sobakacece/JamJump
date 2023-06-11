using Godot;
using System;

public class DeathArea : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("body_entered", this, "KillPlayer");
    }
    public void KillPlayer(Node2D body)
    {
        if (body is Player)
        {
            Player player;
            player = (Player)body;
            player.Death();
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
