using Godot;
using System;

public class Decor : Sprite, ISpawnable
{
    int randomPriority;
    [Export] public int MyRandomPriority { get => randomPriority; set => randomPriority = value; }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
