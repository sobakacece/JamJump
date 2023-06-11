using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class TestLevel : Node2D
{
    Platform cell;
    Player player;
    [Export] float borderOffset;
    float width, cellSpawnPoint, lineHeight = 0;
    [Export] float range = 100, limit = 1000, chunkSpawnHeight = 2000, platformOffset, enemyFreq;
    [Export] int platformAmountLine = 1;
    PackedScene commonPlatform;
    List<PackedScene> sceneList;
    List<PackedScene> enemyList;
    Random rnd = new Random();
    // CollisionShape2D borders;
    int randomWidth, breakableCount = 0;
    ScreenManager screenManager;
	SignalBus signalBus;
    [Signal] public delegate void OnUpdatedDifficulty();
    public override void _Ready()
    {
        width = GetViewportRect().Size.x - borderOffset * 2;

        screenManager = GetNode<ScreenManager>("/root/ScreenManager");
        player = GetNode<Player>("Player");

        commonPlatform = (PackedScene)ResourceLoader.Load("res://Props/Platforms/Platform_long_1.tscn");

        RandomResourceLoader rndLoader = new RandomResourceLoader("res://Props/Platforms/");
        sceneList = rndLoader.ApplyRandom(rndLoader.MySpawnableList);

        RandomResourceLoader enemyLoader = new RandomResourceLoader("res://Props/Enemies/");
        enemyList = rndLoader.ApplyRandom(enemyLoader.MySpawnableList);
        signalBus = GetNode<SignalBus>("/root/SignalBus");
    }
    private void UpdateDifficulty()
    {
        if (platformAmountLine > 1)
            platformAmountLine--;

        if (range < 150)
            range += 10;

        if (enemyFreq > 1500)
            enemyFreq -= 500;
			
        signalBus.EmitSignal("OnUpdatedDifficulty");
    }
    public override void _PhysicsProcess(float delta)
    {
        if (player.GlobalPosition.y <= limit + 1000)
        {
            SpawnChunk();
            limit -= chunkSpawnHeight;
            UpdateDifficulty();
        }
    }
    public void SpawnEnemy()
    {

        Enemy enemy = (Enemy)enemyList[rnd.Next(0, enemyList.Count)].Instance();
        AddChild(enemy);
        enemy.GlobalPosition = new Vector2(rnd.Next((int)-width / 2, (int)width / 2), lineHeight - range / 2);

    }
    public void SpawnChunk()
    {
        while (lineHeight > -chunkSpawnHeight + limit)
        {
            if (Mathf.FloorToInt(lineHeight) % enemyFreq == 0 && lineHeight != 0)
            {
                SpawnEnemy();
            }
            SpawnLine(platformAmountLine, lineHeight);
            lineHeight -= range;
        }
    }
    public void SpawnLine(int platformAmount, float yCoords)
    {
        List<PackedScene> scenesToSpawn = new List<PackedScene>();
        List<Platform> validatedPlatforms = new List<Platform>();
        float slice, start;
        start = -(width / 2); //slice = start Position of the line
        slice = width / (platformAmount); //slice is the block where platform spawns
        for (int i = 0; i < platformAmount; i++)
        {
            scenesToSpawn.Add(sceneList[rnd.Next(0, sceneList.Count)]);
        }
        validatedPlatforms = PlatformValidation(scenesToSpawn);
        foreach (Platform platform in validatedPlatforms)
        {
            if (platform is BreakablePlatform)
                breakableCount++;
            else
                breakableCount = 0;

            float spawnPoint = rnd.Next((int)(start + platformOffset), Mathf.FloorToInt(slice + start - platformOffset));


            if (breakableCount > 3)
            {
                SpawnNode(new Vector2(spawnPoint, lineHeight), (Platform)commonPlatform.Instance());
                breakableCount = 0;
            }
            else

                SpawnNode(new Vector2(spawnPoint, lineHeight), platform);
            start += slice;
        }
    }
    public List<Platform> PlatformValidation(List<PackedScene> scenes)
    {
        List<Platform> validatedPlatforms = new List<Platform>();
        foreach (PackedScene scene in scenes)
        {
            Platform platform = (Platform)scene.Instance();
            if (platform is MovingPlatform)
            {
                validatedPlatforms.Clear();
                validatedPlatforms.Add(platform);
                break;
            }


            validatedPlatforms.Add(platform);
        }


        return validatedPlatforms;

    }
    public void SpawnNode(Vector2 coordinats, Platform platform)
    {
        AddChild(platform);
        platform.GlobalPosition = coordinats;
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
