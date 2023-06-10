using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RandomResourceLoader
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public string MyPath;
    public List<PackedScene> MySpawnableList;
    // Called when the node enters the scene tree for the first time.
    public RandomResourceLoader(string path)
    {
        MyPath = path;
        MySpawnableList = GetAllSpawnable();
    }
    public List<PackedScene> ApplyRandom(List<PackedScene> source)
    {
        List<PackedScene> ranodmList = new List<PackedScene>();
        foreach (PackedScene item in source)
        {
            if (item.Instance() is ISpawnable)
            {
                ISpawnable res = (ISpawnable)item.Instance();
                IEnumerable<PackedScene> temp = null;
                temp = Enumerable.Repeat(item, res.MyRandomPriority);
                ranodmList.AddRange(temp);
            }
            else
            {
                GD.Print($"{item.ToString()} is not Spawnable. Please apply IDespawnable interface to it");
            }
        }
        return ranodmList;
    }
    public List<PackedScene> GetAllSpawnable()
    {
        List<PackedScene> pathes = new List<PackedScene>();
        var dir = new Directory();
        dir.Open(MyPath);

        if (dir != null)
        {
            dir.ListDirBegin();
            string filename = dir.GetNext();
            // GD.Print($"Found file: {filename}");
            while (filename != "")
            {
                if (filename.Contains("tscn"))
                {
                    string tmp = MyPath + filename;
                    // if (tmp.Contains(".remap"))
                    // if (tmp.EndsWith(".remap"))
                    //     tmp = tmp.Trim(".remap"); //so while exporting for some reason Godot renames dynamic pathes as ".remap". So we deleting those;

                    pathes.Add((PackedScene)GD.Load(tmp));
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
}
