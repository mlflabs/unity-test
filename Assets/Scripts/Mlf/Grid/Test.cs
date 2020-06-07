using UnityEngine;
using Mlf.Grid;

namespace Mlf.Grid {

  public class Test : MonoBehaviour {
    private Mlf.Grid.Grid<int> grid;
    private void Start() {
      //grid = new Grid<int>(4,4, 5f, new Vector3(0,0), );
    }

    private void Update() {
      if(Input.GetMouseButtonDown(0)) {
       // grid.SetValue(UtilsInput.GetMouseWorldPosition(), 13);
      }

      if(Input.GetMouseButtonDown(1)) {
        Debug.Log("Getting Value".ToString());
        //int v = grid.GetValue(UtilsInput.GetMouseWorldPosition());
        //Debug.Log(v);
      }
    }
  }
} 