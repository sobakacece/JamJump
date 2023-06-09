using Godot;
using System;
using System.Linq;

public class Cell : Node2D
{
    public VisibilityNotifier2D notifier {get => GetNode<VisibilityNotifier2D>("Notifier");}
    Vector2 coordinates = new Vector2(0, 0);
    public Vector2 MyCoordinates { get => coordinates; set => SetCoordinates(value); }
    Grid2D grid { get => (Grid2D)GetParent(); }
    [Signal] public delegate void CoordinateChanged(Vector2 newCoords, Vector2 oldCoords);
    [Signal] public delegate void CellDespawned(int collumn);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        notifier.Connect("screen_exited", this, "Despawn");
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
}
