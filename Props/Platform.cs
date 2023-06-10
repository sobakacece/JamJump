using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Platform : Node2D, ISpawnable
{
    [Export] int decorNumber = 5;
    public VisibilityNotifier2D notifier { get => GetNode<VisibilityNotifier2D>("Notifier"); }
    Vector2 coordinates = new Vector2(0, 0);
    public Vector2 MyCoordinates { get => coordinates; set => SetCoordinates(value); }
    private int randomPriority = 100;
    [Export] public int MyRandomPriority { get => randomPriority; set => randomPriority = value; }
    // Grid2D grid { get => (Grid2D)GetParent(); }
    [Signal] public delegate void CoordinateChanged(Vector2 newCoords, Vector2 oldCoords);
    [Signal] public delegate void CellDespawned(int collumn);
    Player player;
    List<PackedScene> decorList;
    Sprite sprite;
    Random rnd = new Random();
    bool exited = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        player = GetNode<Player>("/root/TestLevel/Player");
        notifier.Connect("screen_exited", this, "Exited");
        RandomResourceLoader rndLoader = new RandomResourceLoader("res://Props/Decor/");
        decorList = rndLoader.ApplyRandom(rndLoader.MySpawnableList);
        SpawnDecor();
    }
    public void SpawnDecor()
    {
        for (int i = 0; i < 4; i++)
        {
            Decor decor = (Decor)decorList[rnd.Next(decorList.Count)].Instance();
            if (!decor.Despawn())
            {
                this.AddChild(decor);
                int width = Mathf.FloorToInt(sprite.Texture.GetWidth() / 2) - 50;
                decor.Position = new Vector2(rnd.Next(-width, width), -100);

            }
        }
    }
    public void SetCoordinates(Vector2 newCoordintaes)
    {
        if (coordinates != newCoordintaes)
        {
            var _coordinates = coordinates;
            coordinates = newCoordintaes;
            EmitSignal("CoordinateChanged", coordinates, _coordinates);
        }
    }
    public void Despawn()
    {
        EmitSignal("CellDespawned", this.coordinates.x);
        QueueFree();

    }
    public void Exited()
    {
        exited = true;
    }
    public override void _Process(float delta)
    {
        if (exited && this.GlobalPosition.DistanceTo(player.GlobalPosition) > GetViewport().GetVisibleRect().Size.y * 1.5)
        {
            Despawn();
        }
    }
}
