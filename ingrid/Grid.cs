using Godot;
using System;
using System.Collections.Generic;

public class Grid : Container
{
    List<Platform> cells = new List<Platform>();
    Node2D level;
    [Export] Vector2 myCellSize = new Vector2(250, 30);
    int rows = 0, collumns = 0;
    [Export] Vector2 offset = Vector2.Zero;
    string cellPath = "res://addons/ingrid/Cell.tscn";
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        level = GetNode<Node2D>("/root/TestLevel");
        Initialization();
    }
    public void Initialization()
    {
        collumns = Mathf.FloorToInt(RectSize.x / (myCellSize.x + offset.x));
        rows = Mathf.FloorToInt(RectSize.y / (myCellSize.y + offset.y));
        // GD.Print($"collumns {collumns}, rows {rows}, CellSize {myCellSize.ToString()} RectSize {RectSize.ToString()}");

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < collumns; c++)
            {
                SpawnCell(cellPath, r, c);
            }
        }
    }
    public void PlaceOnTop(int collumn)
    {
        GD.Print("PlaceOnTop");
        foreach (Platform item in cells)
        {
            item.SetCoordinates(new Vector2(item.MyCoordinates.y+1, collumn));
        }
        SpawnCell(cellPath, 0, collumn);
    }
    public void SpawnCell(string path, int row, int collumn)
    {
        PackedScene cellScene = (PackedScene)ResourceLoader.Load(path);
        Platform cell = (Platform)cellScene.Instance();
        level.CallDeferred("add_child", cell);
        cell.SetCoordinates(new Vector2(row, collumn));
        cell.GlobalPosition = new Vector2((myCellSize.x + offset.x) * row + RectGlobalPosition.x, (myCellSize.y + offset.y) * collumn + RectGlobalPosition.y - GetViewport().GetVisibleRect().Size.y/2);
        // cell.Connect("CellDespawned", this, "PlaceOnTop");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
         this.RectGlobalPosition = GetNode<Player>("/root/TestLevel/Player").GlobalPosition;
     }
}
