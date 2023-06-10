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
    // float beyondTheScreen;
    // Called hen the ode enters the scene tree for the first time.
    public override void _Ready()
    {
        width = GetViewportRect().Size.x - offset * 2;
        player = GetNode<Player>("Player");
        cellScene = (PackedScene)ResourceLoader.Load("res://Props/Platform_short_1.tscn");
        sceneList = GetAllSpawnable("res://Props/");
        // beyondTheScreen = player.GlobalPosition.y - GetViewport().GetVisibleRect().Size.y;
        sceneList =  ApplyRandom(sceneList);
    }
    public List<PackedScene> ApplyRandom(List<PackedScene> source)
    {
        List<PackedScene> ranodmList = new List<PackedScene>();
        foreach (PackedScene item in source)
        {
            ISpawnable platform = (ISpawnable)item.Instance();
            var temp = Enumerable.Repeat(item, platform.MyRandomPriority);
            ranodmList.AddRange(temp);
            GD.Print(temp.ToList().ToString()); 
        }
        return ranodmList;
    }
    // public override void _Input(InputEvent @event)
    // {
    //    this.fOC
    // }
    public List<PackedScene> GetAllSpawnable(string path)
    {
        List<PackedScene> pathes = new List<PackedScene>();
        var dir = new Directory();
        dir.Open(path);
        
        if (dir != null)
        {
            dir.ListDirBegin();
            string filename = dir.GetNext();
            // GD.Print($"Found file: {filename}");
            while (filename != "")
            {
                if (filename.Contains("tscn"))
                {
                    string tmp = path + filename;
                    // if (tmp.Contains(".remap"))
                    // if (tmp.EndsWith(".remap"))
                    //     tmp = tmp.Trim(".remap"); //so while exporting for some reason Godot renames dynamic pathes as ".remap". So we deleting those;

                    pathes.Add((PackedScene)ResourceLoader.Load(tmp));
                    // GD.Print($"Found file: {filename}");
                }

                filename = dir.GetNext();
            }
        }
        else
        {
            GD.Print("Check name of dir Cells");
        }
        return pathes;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
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
}
