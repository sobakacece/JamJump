using Godot;
using System;

public class TestLevel : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Cell cell;
    Player player;
    float width, cellSpawnPoint, y = 0;
    [Export] float range, limit = 0;
    PackedScene cellScene;
    Random rnd = new Random();
    CollisionShape2D borders;
    int randomWidth;
    float beyondTheScreen;
    // Called hen the ode enters the scene tree for the first time.
    public override void _Ready()
    {
        borders = GetNode<CollisionShape2D>("Borders/CollisionShape2D");
        RectangleShape2D rect = (RectangleShape2D)borders.Shape;
        width = rect.Extents.x;
        player = GetNode<Player>("Player");
        cellScene = (PackedScene)ResourceLoader.Load("res://ingrid/Cell.tscn");
        beyondTheScreen = player.GlobalPosition.y - GetViewport().GetVisibleRect().Size.y;
        Initialize();
    }
    public void Initialize()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        // if (player.GlobalPosition.y < cellSpawnPoint - range)
        // {
        //     GD.Print($"Player: {player.GlobalPosition}, cell: {cellSpawnPoint - range}");
        //     GD.Print("SPAWNED");
        //     SpawnNode(new Vector2(rnd.Next((int)-width / 2, (int)width / 2), cellSpawnPoint - GetViewport().GetVisibleRect().Size.y / 2));
        //     GD.Print($"cell position{cell.GlobalPosition.ToString()}");
        // }
        if (player.GlobalPosition.y <= limit)
        {
            SpawnChunk();
            limit -= 2000;
        }
        // if (y - range * 2 < -3000)
        // {
        //     GD.Print($"y: {y} range * 2: {range * 2}");
        //     limit *= 2;
        // }
    }
    public void SpawnChunk()
    {
        while (y > -3000+limit)
        {
            SpawnNode(new Vector2(rnd.Next((int)-width / 2, (int)width / 2), y));
            GD.Print(y);
            y -= range;
        }
    }
    public void SpawnNode(Vector2 coordinats)
    {
        cell = (Cell)cellScene.Instance();
        AddChild(cell);
        // cell.GlobalPosition = new Vector2(rnd.Next((int)-width / 2, (int)width / 2), player.GlobalPosition.y - GetViewport().GetVisibleRect().Size.y);
        cell.GlobalPosition = new Vector2(coordinats);

    }
}
