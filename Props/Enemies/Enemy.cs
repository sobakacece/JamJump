using Godot;
using System;

public class Enemy : KinematicBody2D, ISpawnable
{
    [Export] int randomPriority;
    public int MyRandomPriority { get => randomPriority; set => randomPriority = value; }
    bool exited = false;
    Player player;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Area2D area = GetNode<Area2D>("Area2D");
        area.Connect("body_entered", this, "OnCollision");
        VisibilityNotifier2D notifer = GetNode<VisibilityNotifier2D>("Notifier");
        player = GetNode<Player>("/root/TestLevel/Player");
        notifer.Connect("screen_exited", this, "Exited");
    }
    public void OnCollision(Node2D body)
    {
        if (body is Player)
        {
            Player player = (Player)body;
            player.Death();
        }
    }
    public void Exited()
    {
        exited = true;
    }
    public void Despawn()
    {
        QueueFree();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (exited && this.GlobalPosition.DistanceTo(player.GlobalPosition) > GetViewport().GetVisibleRect().Size.y * 1.5)
        {
            Despawn();
        }
    }
}
