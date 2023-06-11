using Godot;
using System;

public class Propeller : Decor
{

    [Export] float force, speed;
    Area2D area;
    Player player;

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
        if (body is Player)
        {
            player = (Player)body;
            player.animationPlayer.Play("propeller");
            GetNode<AudioStreamPlayer>("Flying").Play();
            player.velocity = new Vector2(player.velocity.x, -force);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

}
