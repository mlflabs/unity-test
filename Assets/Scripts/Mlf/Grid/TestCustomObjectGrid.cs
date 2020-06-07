using UnityEngine;
using Mlf.Grid;

namespace Mlf.Grid {

  public class TestCustomObjectGrid : MonoBehaviour {
    private Grid<GenericObject>  grid;



    private void Start() {
      grid = new Grid<GenericObject>(4,4, 5f, new Vector3(0,0), (Grid<GenericObject> g, int x, int y) =>  new GenericObject(g, x, y));
    }

    private void Update() {
      if(Input.GetMouseButtonDown(0)) {
        GenericObject value = grid.GetGridObject(UtilsInput.GetMouseWorldPosition());
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

    
  }

  public class GenericObject 
  {
    private const int MIN = 0;
    private const int MAX = 100;
    public int value = 0;
    private Grid<GenericObject> grid;
    private int x;
    private int y;


    public GenericObject(Grid<GenericObject> grid, int x, int y)
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
  