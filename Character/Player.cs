using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public AnimationPlayer animationPlayer;
    private ScreenManager screenManager;
    private CollisionShape2D collisionShape;
    private int gravity;
    public bool boosted = false;
    [Export] public float jumpVelocity = -1000;
    [Export] public float speed;
    public Vector2 velocity;
    List<AudioStreamPlayer> sounds = new List<AudioStreamPlayer>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gravity = (int)ProjectSettings.GetSetting("physics/2d/default_gravity") * 10;
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        screenManager = GetNode<ScreenManager>("/root/ScreenManager");
        animationPlayer.Connect("animation_finished", this, "AnimationFinished");

        sounds = GetNode<Node>("Sounds").GetChildren().OfType<AudioStreamPlayer>().ToList();
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 direction = InputGetVector("left", "right", "ui_up", "ui_down");

        if (!IsOnFloor())
            velocity.y += delta * gravity;
        else
        {
            velocity.y = jumpVelocity;
            sounds.Find(x => x.Name == "Jump").Play();
        }
        if (direction.x != 0)
        {
            velocity.x = direction.x * speed;
        }
        else
        {
            velocity.x = Mathf.MoveToward(velocity.x, 0, speed);
        }
        // if (boosted)
        // {
        //     velocity.y = Mathf.MoveToward(velocity.y, -4000, 1000);
        // }
        CheckIframes();
        MoveAndSlide(velocity, Vector2.Up);

    }
    public void AnimationFinished(String anim_name)
    {
        if (anim_name == "dead")
        {
            screenManager.gameOver.Visible = true;
            GetTree().Paused = true;
        }
    }

    public void Death()
    {
        sounds.Find(x => x.Name == "Dead").Play();
        animationPlayer.Play("dead");
    }
    public Vector2 InputGetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = 0.5f)
    {
        var strength = new Vector2(
            Input.GetActionStrength(positiveX) - Input.GetActionStrength(negativeX),
            Input.GetActionStrength(positiveY) - Input.GetActionStrength(negativeY)
        ).Normalized();
        return strength.Length() > deadzone ? strength : Vector2.Zero;
    }
    private void CheckIframes()
    {
        if (velocity.y < jumpVelocity - 50)
        {
            collisionShape.Disabled = true;
        }
        else
        {
            collisionShape.Disabled = false;
        }
    }

}

// Called every frame. 'delta' is the elapsed time since the previous frame.

