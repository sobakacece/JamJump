using Godot;
using System;

public class ScreenManager : Node
{
    public GameScreen gamePaused, gameOver;
    float finalScore;
    public float MyFinalScore {get => finalScore; set => finalScore = value;}
    public override void _Ready()
    {
        gamePaused = AddGlobalScreen("res://UI/GamePaused.tscn");
        gameOver = AddGlobalScreen("res://UI/GameOver.tscn");
        gameOver.Connect("visibility_changed", this, "UpdateFinalScore");
        PauseMode = Node.PauseModeEnum.Process;
    }
    public GameScreen AddGlobalScreen(string path)
    {
        GameScreen MyScreen = new GameScreen();
        PackedScene Screen = (PackedScene)ResourceLoader.Load(path);
        MyScreen = (GameScreen)Screen.Instance();
        this.AddChild(MyScreen);
        MyScreen.Visible = false;
        return MyScreen;
    }
    public void UpdateFinalScore()
    {
        GameOver gameOverScreen = (GameOver) gameOver;
        gameOverScreen.scoreLabel.Text = $"Your score is: {finalScore.ToString("000000")}";
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("restart"))
        {
            gameOver.Restart();
            gameOver.Visible = false;
            gamePaused.Visible = false;
        }
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
