using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mlf.Utils;
using Mlf.Grid;

namespace Mlf.Grid
{

  public class Grid<TGridObject>
  {
    private bool showDebug;
    public int debugFontSize = 6;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
      public int x;
      public int y;
    }

    private int width;
    public int Width
    {
      get
      {
        return width;
      }
    }
    private int height;
    public int Height
    {
      get
      {
        return height;
      }
    }
    private float cellSize;
    public float CellSize
    { 
      get
      {
        return cellSize; 
      }
    }

    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    internal void AddValue(Vector3 position, int v1, int v2, int v3)
    {
      throw new NotImplementedException();
    }

    private TextMesh[,] debugTextArray;



    public Grid(int width, int height, float cellSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug = false)
    {
      CreateGrid(width, height, cellSize, new Vector3(), createGridObject, showDebug);
    }

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug = false)
    {
      CreateGrid(width, height, cellSize, originPosition, createGridObject, showDebug);
    }

    private void CreateGrid(int _width, int _height, float _cellSize,
            Vector3 _originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug)
    {
      width = _width;
      height = _height;
      cellSize = _cellSize;
      originPosition = _originPosition;
      this.showDebug = showDebug;

      gridArray = new TGridObject[width, height];
      debugTextArray = new TextMesh[width, height];


      for (int x = 0; x < gridArray.GetLength(0); x++)
      {
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
          gridArray[x, y] = createGridObject(this, x, y);
        }
      }


      // what follows is for debuging
      if (!showDebug) return;

      drawDebugGrid();
      

    }

    public void drawDebugGrid()
    {
      Debug.Log("Grid System: " + width + " " + height);

      // Transform fontTransform = empt.transform;

      for (int x = 0; x < gridArray.GetLength(0); x++)
      {
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
          debugTextArray[x, y] = Mlf.Utils.Utils.CreateWorldText(gridArray[x, y]?.ToString(), null,
            GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f,
            debugFontSize, Color.white, TextAnchor.MiddleCenter);
          Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
          Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

        }
      }
      Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
      Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);


      OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
      {
        debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
      };
    }

    public GridCell GetGridPosition(Vector3 worldPosition)
    {
      return new GridCell(Mathf.FloorToInt((worldPosition - originPosition).x / cellSize),
                          Mathf.FloorToInt((worldPosition - originPosition).y / cellSize));
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
    {
      x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
      y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
      return new Vector3(x, y) * cellSize + originPosition;
    }



    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
      GridCell gc = GetGridPosition(worldPosition);
      SetGridObject(gc.x, gc.y, value);

    }


    public void SetGridObject(int x, int y, TGridObject value)
    {
      Debug.Log("Grid SetValue: " + value);
      if (x >= 0 && y >= 0 && x < width && y < height)
      {
        //gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
        gridArray[x, y] = value;
        debugTextArray[x, y].text = gridArray[x, y].ToString();
        TriggerGridObjectChanged(x, y);
      }

    } 





    public TGridObject GetGridObject(Vector3 worldPosition)
    {
      int x, y;
      GetGridPosition(worldPosition, out x, out y);
      return GetGridObject(x, y);
    }



    public TGridObject GetGridObject(int x, int y)
    {

      //return 10;
      if (x >= 0 && y >= 0 && x < width && y < height)
      {
        return gridArray[x, y];
      }
      else
      {
        return default;
      }
    }


    public void TriggerGridObjectChanged(int x, int y)
    {
      OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }



  }

}
