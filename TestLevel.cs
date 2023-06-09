using Godot;
using System;

public class TestLevel : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Cell cell;
    Player player;
    float width, height, spawnPoint;
    PackedScene cellScene;
    Random rnd = new Random();
    CollisionShape2D borders;
    // Called hen the ode enters the scene tree for the first time.
    public override void _Ready()
    {
        borders = GetNode<CollisionShape2D>("Borders/CollisionShape2D");
        RectangleShape2D rect = (RectangleShape2D)borders.Shape;
        width = rect.Extents.x;
        height = 300f;
        player = GetNode<Player>("Player");
        spawnPoint = player.GlobalPosition.y;
        cellScene = (PackedScene)ResourceLoader.Load("res://ingrid/Cell.tscn");
    }
    public void GridInit()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (player.GlobalPosition.y < spawnPoint + height)
        {
            GD.Print("SPAWNED");
            Cell cell = (Cell)cellScene.Instance();
            AddChild(cell);
            cell.GlobalPosition = new Vector2(rnd.Next((int)-width / 2, (int)width / 2), player.GlobalPosition.y - GetViewport().GetVisibleRect().Size.y);
            GD.Print($"cell position{cell.GlobalPosition.ToString()}");
            spawnPoint -= height;
        }
    }
}
