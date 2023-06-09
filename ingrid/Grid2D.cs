using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Grid2D : ColorRect
{

    #region Public members
    Vector2 cellUnits;
    public Vector2 MyCellUnits
    { get => cellUnits == null ? new Vector2(1, 1) : cellUnits; set => cellUnits = value; }

    Vector2 cellSize;
    public Vector2 MyCellSize
    { get => cellSize == null ? OS.WindowSize / 2 : cellSize; set => cellSize = value; }
    public bool cellsFitted = true;
    public string cellDelegate = "res://addons/ingrid/Cell.tscn";
    #endregion
    #region  private memebers
    List<Cell> cells = new List<Cell>();
    int rows = 0;
    int cols = 0;
    int cellSpawning = 0;
    [Export] Vector2 offset = new Vector2(0, 0);

    Rect2 fill;
    Rect2 bounds;
    PackedScene cellScene;
    #endregion
    #region  Signals
    [Signal] delegate void initialized();
    [Signal] delegate void Moved(Vector2 newPosition);
    #endregion

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fill = new Rect2(this.RectPosition, this.RectSize);
        bounds = new Rect2(this.RectPosition, this.RectSize);
        cellScene = (PackedScene)ResourceLoader.Load(cellDelegate);
        Connect("resized", this, "OnResize");

    }
    private bool Validated()
    {
        return cellSpawning == 0 && cells.Count > 0 && cells.Count == rows * cols;
    }
    public void Initialization()
    {
        UpdateFills();
        UpdateOffset();

        int width = (int)fill.Size.x;
        int height = (int)fill.Size.y;

        Clear();

        cols = (int)Mathf.Ceil(width / cellSize.x);
        rows = (int)Mathf.Ceil(height / cellSize.y);

        if (cols * rows < 4)
        {
            GD.Print("error, to little rows or collumns");
            return;
        }
        int cellAmount = rows * cols;
        cellSpawning = cellAmount;

        Resource res = ResourceLoader.Load(cellDelegate);
        for (var i = 0; i < cellAmount; i++)
        {
            OnCellSpawned(res.Duplicate());
        }

    }
    private void OnCellSpawned(Resource res)
    {
        if (res == null)
            return;
        PackedScene resScene = (PackedScene)res;
        Cell cellInstance = (Cell)resScene.Instance();

        cellSpawning--;

        if (!IsInstanceValid(this) || cellInstance == null)
        {
            GD.Print($"gRID2d ERROR bad cell {cellInstance.ToString()}, {cellSpawning}");
            return;
        }
        AddChild(cellInstance);

        cells.Add(cellInstance);

        if (cellSpawning == 0)
        {
            Arrange();
            EmitSignal("initialized");
        }
    }
    private void Arrange()
    {
        if (!Validated())
            return;

        UpdateOffset();
        Cell cell = null;
        int index = 0;
        for (var x = 0; x < cols; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                cell = cells.ElementAt(index);
                cell.MyCoordinates = new Vector2(x, y) * cellUnits;
                cell.Position = offset + new Vector2(x * cellSize.x, y * cellSize.y);
                index++;
            }
        }
        Move(Vector2.Zero);
    }
    public void Warp(Vector2 coordinate)
    {
        if (!Validated())
            return;

        Vector2 tmpCoords = new Vector2(coordinate.x - Mathf.FloorToInt(cols / 2), coordinate.y - Mathf.FloorToInt(rows / 2));
        SortCells();

        int xmod = 0, ymod = -1;
        Cell tmpCell, center = null;
        for (var i = 0; i < cells.Count; i++)
        {
            if (i % cols == 0)
                xmod = 0; ymod += 1;
            tmpCell = cells[i];
            tmpCell.MyCoordinates = new Vector2(tmpCoords.x + xmod, tmpCoords.y + ymod) * cellUnits;

            if (tmpCell.MyCoordinates == coordinate)
                center = tmpCell;
            xmod++;
        }
        Vector2 centralPos = center.Position;
        var x = (-Mathf.Abs(centralPos.x));
        var y = (Mathf.Abs(centralPos.y));

        if (cellSize.x > RectSize.x)
            x = -x;
        if (cellSize.y > RectSize.y)
            y = -y;

        Move(new Vector2(x + RectSize.x / 2 - cellSize.x / 2, y + RectSize.y / 2 - cellSize.y / 2));
    }
    public void Move(Vector2 coordinate)
    {
        if (!Validated())
            return;

        float nx, ny;
        bool update = false, doSort = false;
        Vector2 limit_tl = bounds.Position;
        Vector2 limit_br = limit_tl + bounds.Size - cellSize;

        Vector2 swap;
        foreach (Cell cell in cells)
        {
            if (cell == null)
                break;
            update = false;
            nx = cell.Position.x + coordinate.x;
            ny = cell.Position.y + coordinate.y;

            swap = cell.MyCoordinates;

            if (nx < limit_tl.x)
            {
                cell.Position += new Vector2(cols * cellSize.x, 0);
                swap.x = cell.MyCoordinates.x + cols * cellUnits.x;
                update = true;
            }
            else if (ny < limit_tl.y)
            {
                cell.Position += new Vector2(0, rows * cellSize.y);
                swap.y = cell.MyCoordinates.x + rows * cellUnits.y;
                update = true;
            }
            else if (nx > limit_br.x)
            {
                cell.Position -= new Vector2(cols * cellSize.x, 0);
                swap.x = cell.MyCoordinates.x - cols * cellUnits.x;
                update = true;
            }
            else if (nx < limit_tl.x)
            {
                cell.Position -= new Vector2(0, rows * cellSize.y);
                swap.y = cell.MyCoordinates.x - rows * cellUnits.y;
                update = true;
            }

            cell.Position += coordinate;
            if (update)
            {
                doSort = true;
                cell.MyCoordinates = swap;
                update = false;
            }

        }
        if (doSort)
            SortCells();
        EmitSignal("Moved", coordinate);
    }
    private Cell GetVisibleCell(Vector2 coordinate)
    {
        if (RectSize.x < 0 || coordinate.x > RectSize.x)
            return null;
        if (RectSize.y < 0 || coordinate.y > RectSize.y)
            return null;

        foreach (Cell cell in cells)
        {
            if (coordinate >= cell.Position && coordinate <= cell.Position + cellSize)
                return cell;
        }
        return null;
    }
    public void AutoCellFit()
    {
        if (!Validated())
            return;

        UpdateFills();
        UpdateOffset();

        int width = (int)fill.Size.x;
        int height = (int)fill.Size.y;

        int tmpCols = Mathf.CeilToInt(width / cellSize.x);
        int tmpRows = Mathf.CeilToInt(height / cellSize.y);
        int tmpCellAmount = rows * cols - tmpCols * tmpRows;

        int colsAmount = tmpCols - cols;
        int rowsAmount = tmpRows - cols;

        if (Mathf.Abs(tmpCellAmount) > 0)
        {
            Cell tmpCell;
            PackedScene res;
            SortCells();

            Cell tl = cells.First();
            cellSpawning = tmpCellAmount;

            if (tmpCellAmount > 0)
                res = (PackedScene)ResourceLoader.Load(cellDelegate);

            if (colsAmount < 0)
            {
                for (int j = 0; j < colsAmount; j++)
                {
                    int index = 0, ei = 0;
                    for (int i = 0; i < rowsAmount; i++)
                    {
                        index = (i++) * cols + ei - 1;
                        tmpCell = cells[index];
                        tmpCell.Free();
                        cells[index] = null;
                        cells.Remove(cells[index]);
                        ei--;
                        cellSpawning++;
                    }
                    cols--;
                }
            }
            if (colsAmount > 0)
            {
                for (int j = 0; j < colsAmount; j++)
                {
                    for (int i = 0; i < rowsAmount; i++)
                    {
                        tmpCell = (Cell)cellScene.Instance();
                        cellSpawning--;
                        AddChild(tmpCell);
                        tmpCell.MyCoordinates = new Vector2(tl.MyCoordinates.x + cols, tl.MyCoordinates.y + i) * cellUnits;
                        tmpCell.Position = new Vector2(tl.Position.x + (cols * cellSize.x), tl.Position.y + (i * cellSize.y));
                        cells.Add(tmpCell);
                    }
                    cols++;
                }
            }
            if (rowsAmount < 0)
            {
                for (var i = 0; i < cols * rowsAmount; i++)
                {
                    tmpCell = cells.Last();
                    cells.Remove(cells.Last());
                    tmpCell.Free();
                    cellSpawning++;
                }
                rows += rowsAmount;
            }
            if (rowsAmount > 0)
            {
                for (var j = 0; j < rowsAmount; j++)
                {
                    for (var i = 0; i < cols; i++)
                    {
                        tmpCell = (Cell)cellScene.Instance();
                        cellSpawning--;
                        AddChild(tmpCell);
                        tmpCell.MyCoordinates = new Vector2(tl.MyCoordinates.y + i, tl.MyCoordinates.y + rows) * cellUnits;
                        tmpCell.Position = new Vector2(tl.Position.x + (i * cellSize.x), tl.Position.y + (rows * cellSize.y));
                        cells.Add(tmpCell);
                    }
                    rows++;
                }
            }
            SortCells();
            UpdateFills();
            UpdateOffset();

        }
    }
    public void OnResize()
    {
        if (cellsFitted)
        {
            AutoCellFit();
        }
    }

    private void Clear()
    {
        foreach (Cell cell in cells)
        {
            cells.Remove(cell);
        }
        rows = 0;
        cols = 0;
        cellSpawning = 0;
    }
    public void UpdateFills()
    {
        Vector2 fillBox = cellSize * 3;
        Vector2 boundsBox = new Vector2(fillBox.x + cellSize.x, fillBox.y + cellSize.y);

        Rect2 rect = new Rect2(this.RectPosition, RectSize);
        var fillArea = rect.GrowIndividual(fillBox.x, fillBox.y, fillBox.x, fillBox.y);
        var boundsArea = rect.GrowIndividual(boundsBox.x, boundsBox.y, boundsBox.x, boundsBox.y);

        fill.Position = fillArea.Position;
        fill.Size = fillArea.Size;

        bounds.Position = boundsArea.Position;
        bounds.Size = bounds.Size;
    }
    public void UpdateOffset()
    {
        offset.x = -(rows * cellSize.x - RectSize.x) / 2;
        offset.y = -(cols * cellSize.y - RectSize.y) / 2;
    }
    public void SortCells()
    {
        cells.OrderBy(x => x.Position.y);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
