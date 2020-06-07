


using UnityEngine;


namespace Mlf.Grid {

  public class GridHeatMap { 

    public const int HEATMAP_MAX_VALUE = 100;
    public const int HEATMAP_MIN_VALUE = 0;

    private bool updateMesh;

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private bool debug;
    private int[,] gridArray;

    private TextMesh[,] debugTextArray;


    public GridHeatMap(int width, int height, float cellSize, Vector3 originPosition, bool debug) {
      this.width = width;
      this.height = height;
      this.debug = debug;
      this.cellSize = cellSize;
      this.originPosition = originPosition; 

      gridArray = new int[width, height];
      debugTextArray = new TextMesh[width, height];

      if (!debug) return;

      for(int x = 0; x < gridArray.GetLength(0); x++) {
        for(int y = 0; y < gridArray.GetLength(1); y ++) {
          debugTextArray[x, y] =  Mlf.Utils.Utils.CreateWorldText(gridArray[x,y].ToString(), null, 
            GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 12, Color.white, TextAnchor.MiddleCenter);
          
          DrawDebug(x, y, x+1, y+1);
          
        }
      }

      DrawDebug(0, gridArray.GetLength(1), gridArray.GetLength(0), gridArray.GetLength(1));
      DrawDebug(gridArray.GetLength(0), 0, gridArray.GetLength(0), gridArray.GetLength(1));
    }

    public void SetValue(int x, int y, int value) {
      if(x < 0 || y < 0) return;
      if(x > width - 1 ||  y > height - 1) return;

      gridArray[x, y] = Mathf.Clamp(value, HEATMAP_MIN_VALUE, HEATMAP_MAX_VALUE);
      debugTextArray[x, y].text = gridArray[x, y].ToString();
    }

    public void SetValue( Vector3 worldPosition, int value) {
      int x, y;
      GetXY(worldPosition, out x, out y);

      SetValue(x, y, value);
    }

    private Vector3 GetWorldPosition(int x, int y) {
      return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) {
      x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
      y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private void DrawDebug(int x, int y, int tox, int toy) {
      if(!debug) return;

      Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, toy), Color.white, 100f);
      Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(tox,y), Color.white, 100f);
    }

    public int GetValue(int x, int y) {
      if(x < 0 || y < 0) return 0;
      if(x > width - 1 ||  y > height - 1) return 0;

      return gridArray[x, y];
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    
  }
}