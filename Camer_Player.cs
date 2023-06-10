using Godot;
using System;

public class Camer_Player : Camera2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Player player;
    float tmp = 1;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<Player>("/root/TestLevel/Player");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        float tmp = -1;
        if (player.GlobalPosition.y < tmp)
            tmp = player.GlobalPosition.y;
        this.GlobalPosition = new Vector2(0, tmp);
    }
}
