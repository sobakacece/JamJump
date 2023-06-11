using Godot;
using System;

public class SignalBus : Node
{
    [Signal] public delegate void OnUpdatedDifficulty();
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
