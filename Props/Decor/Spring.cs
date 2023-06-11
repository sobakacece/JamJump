using Godot;
using System;

public class Spring : Decor
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] public float springForce;
    Area2D area;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        area = GetNode<Area2D>("Area2D");
        area.Connect("body_entered", this, "OnCollision");
        MySpawnHeight = -50;
    }
    public void OnCollision(Node2D body)
    {
        Player player;
        if (body is Player)
        {
           player = (Player)body;
           player.velocity = new Vector2(player.velocity.x, -springForce);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
