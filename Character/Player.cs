using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private AnimationPlayer animationPlayer;
    private ScreenManager screenManager;
    private int gravity;
    [Export] public float jumpVelocity = -1000;
    [Export] public float speed;
    public Vector2 velocity;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gravity = (int)ProjectSettings.GetSetting("physics/2d/default_gravity") * 10;
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        screenManager = GetNode<ScreenManager>("/root/ScreenManager");
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 direction = InputGetVector("left", "right", "ui_up", "ui_down");

        if (!IsOnFloor())
            velocity.y += delta * gravity;
        else
            velocity.y = jumpVelocity;
        if (direction.x != 0)
        {
            velocity.x = direction.x * speed;
            // GD.Print(direction);
        }
        else
        {
            velocity.x = Mathf.MoveToward(velocity.x, 0, speed);
        }
        // GD.Print(velocity.y);
        MoveAndSlide(velocity, Vector2.Up);

    }

    public void Death()
    {
        animationPlayer.Play("dead");
        screenManager.gameOver.Visible = true;
        GetTree().Paused = true;
    }
    public Vector2 InputGetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = 0.5f)
    {
        var strength = new Vector2(
            Input.GetActionStrength(positiveX) - Input.GetActionStrength(negativeX),
            Input.GetActionStrength(positiveY) - Input.GetActionStrength(negativeY)
        ).Normalized();
        return strength.Length() > deadzone ? strength : Vector2.Zero;
    }
}

// Called every frame. 'delta' is the elapsed time since the previous frame.

