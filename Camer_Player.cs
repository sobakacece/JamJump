using Godot;
using System;

public class Camer_Player : Camera2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Player player;
    // float highestPosition = 0;
    float highBound = 0, lowBound = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<Player>("/root/TestLevel/Player");
        highBound = player.Position.y;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        lowBound = highBound + 300;
        if (player.Position.y < lowBound && player.Position.y > highBound) // if player is higher than lowerBound
        {
            this.GlobalPosition = new Vector2(0, player.Position.y - 100);
        }
        else if (player.Position.y < highBound)
        {
            highBound = player.Position.y - 100;
            this.GlobalPosition = new Vector2(0, highBound);
        }

        // float currPosition = -player.Position.y;
        // float lowestPosition = currPosition + 100;
        // if (currPosition > highestPosition && highestPosition < lowestPosition)
        //     highestPosition = currPosition;

        // this.GlobalPosition = new Vector2(0, -highestPosition);


    }
}
