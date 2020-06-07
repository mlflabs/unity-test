using UnityEngine;
using Mlf.Grid;
using Mlf.Utils;

namespace Mlf.Grid {

  public class TestHeatMapVisualGrid : MonoBehaviour {
    private Grid<HeatMapObject>  grid;

    [SerializeField] Mesh mesh;
    private bool updateMesh;

    private void Start() {
      grid = new Grid<HeatMapObject>(4,4, 5f, new Vector3(0,0), (Grid<HeatMapObject> g, int x, int y) =>  new HeatMapObject(g, x, y));
    }

    private void Awake()
    {
      mesh = new Mesh();
      GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update() {
      if(Input.GetMouseButtonDown(0)) {
        HeatMapObject value = grid.GetGridObject(UtilsInput.GetMouseWorldPosition());
        Debug.Log(value);
        if(value != null)
        { 
          value.AddValue(5); 
        }
        
      }

      if(Input.GetMouseButtonDown(1))
      {
        Debug.Log("Getting Value".ToString());
        //int v = grid.GetValue(UtilsInput.GetMouseWorldPosition());
       // Debug.Log(v);
      }
    }

    private void LastUpdate()
    {
      if (updateMesh)
      {
        updateMesh = false;
        UpdateHeatMapVisual();
      }
    }

    public void SetGrid(Grid<HeatMapObject> grid)
    {
      this.grid = grid;
      UpdateHeatMapVisual();

      grid.OnGridValueChanged += Grid_OnGridValueChanged;

    }

    private void Grid_OnGridValueChanged(object sender, Grid<HeatMapObject>.OnGridValueChangedEventArgs e)
    {
      updateMesh = true;
    }


    public void UpdateHeatMapVisual()
    {
      UtilsMesh.CreateEmptyMeshArrays(grid.Width * grid.Height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

      for(int x = 0; x < grid.Width; x++)
      {
        for(int y = 0; y < grid.Height; y++)
        {
          int index = x * grid.Height + y;
          Vector3 quadSize = new Vector3(1, 1) * grid.CellSize;

          HeatMapObject gridCell = grid.GetGridObject(x, y);

          Vector2 gridValueUV = new Vector2(gridCell.GetValueNormalized(), 0f);
          //MeshUtils.AddToMeshArray() watch mesh video
        }
      }
    }
  }

  public class HeatMapObject 
  { 
    private const int MIN = 0; 
    private const int MAX = 100;
    public int value = 0;
    private Grid<HeatMapObject> grid;
    private int x;
    private int y;


    public HeatMapObject(Grid<HeatMapObject> grid, int x, int y)
    {
      this.grid = grid;
      this.x = x;
      this.y = y;
    }

    public void AddValue(int addValue)
    {
      value = Mathf.Clamp(value + addValue, MIN, MAX);
      grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
      return (float)value / MAX;
    }

    public override string ToString()
    {
      return value.ToString();
    }
  }
   
  /*
   * public void AddValue(int x, int y, int value)
    {
      SetValue(x, y, GetValue(x, y) + value);
    }

    public void AddValue(Vector3 worldposition, int value, int fullvaluerange, int totalrange)
    {
      int lowerValueAmount = Mathf.RoundToInt((float)value / (totalrange - fullvaluerange));

      GetXY(worldposition, out int originX, out int originY);

      for(int x = 0; x < totalrange; x++)
      {
        for(int y = 0; y < totalrange - x; y++)
        {
          int radius = x + y;
          int addValueAmount = value;
          if(radius > fullvaluerange)
          {
            addValueAmount -= lowerValueAmount * (radius - fullvaluerange);
          }
          AddValue(originX + x, originY + y, addValueAmount);
          if(x != 0){
            AddValue(originX - x, originY + y, addValueAmount);
          }

          if(y != 0)
          {
            AddValue(originX + x, originY - y, addValueAmount);
            if (y != 0)
            {
              AddValue(originX - x, originY - y, addValueAmount);
            }
          }
        }
      }

    }

    */
} 
  