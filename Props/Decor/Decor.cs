using Godot;
using System;

public class Decor : Sprite, ISpawnable
{
    int randomPriority;
    [Export] public int MyRandomPriority { get => randomPriority; set => randomPriority = value; }
    public bool despawned = false;
    int chanceToSpawn;
    Random rnd = new Random();
    [Export] public int MyChanceToSpawn { get => chanceToSpawn; set => chanceToSpawn = value; }
    [Export] private float heightSpawn = -80;
    public float MySpawnHeight { get => heightSpawn; set => heightSpawn = value;}

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }
    public bool Despawn()
    {
        if (chanceToSpawn < rnd.Next(0,100))
        {
            return true;
        }
        return false;
    }
}
