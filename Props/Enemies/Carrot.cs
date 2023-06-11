using Godot;
using System;

public class Carrot : Enemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";\
    protected int gravity = (int)ProjectSettings.GetSetting("physics/2d/default_gravity") * 10;

    Vector2 velocity;
    [Export] float jumpVelocity = -400;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Timer timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, "Jump");
    }
    public void Jump()
    {
        if (IsOnFloor())
        velocity.y = jumpVelocity;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (!IsOnFloor())
        {
            velocity.y += delta * gravity;
        }
        MoveAndSlide(velocity, Vector2.Up);
    }
}
