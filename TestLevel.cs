using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class TestLevel : Node2D
{
    Platform cell;
    Player player;
    [Export] float offset;
    float width, cellSpawnPoint, y = 0;
    [Export] float range, limit = 0;
    PackedScene cellScene;
    List<PackedScene> sceneList;
    Random rnd = new Random();
    // CollisionShape2D borders;
    int randomWidth;
    ScreenManager screenManager;
    public override void _Ready()
    {
        
        width = GetViewportRect().Size.x - offset * 2;

        screenManager = GetNode<ScreenManager>("/root/ScreenManager");
        player = GetNode<Player>("Player");

        RandomResourceLoader rndLoader = new RandomResourceLoader("res://Props/Platforms/");
        sceneList = rndLoader.ApplyRandom(rndLoader.MySpawnableList);
    }
    public override void _PhysicsProcess(float delta)
    {
        if (player.GlobalPosition.y <= limit)
        {
            SpawnChunk();
            limit -= 2000;
        }
    }
    public void SpawnChunk()
    {
        while (y > -3000 + limit)
        {
            SpawnNode(new Vector2(rnd.Next((int)-width / 2, (int)width / 2), y), sceneList[rnd.Next(0, sceneList.Count)]);
            GD.Print(y);
            y -= range;
        }
    }
    public void SpawnNode(Vector2 coordinats, PackedScene scene)
    {
        cell = (Platform)scene.Instance();
        AddChild(cell);
        cell.GlobalPosition = new Vector2(coordinats);
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("ui_cancel"))
        {
            screenManager.gamePaused.Visible = true;
            GetTree().Paused = true;
        }
    }
}
