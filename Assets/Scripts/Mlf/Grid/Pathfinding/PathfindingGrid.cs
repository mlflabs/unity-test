using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Mlf.Grid.Pathfinding
{
  public class PathfindingGrid : MonoBehaviour
  {

    private Pathfinding pathfinding;

    public int width = 10;
    public int height = 10;
    public float cellsize = 1f;
    public bool showGizmos = true;
    public bool updateVisual;

    void Start()
    {
      //editor ui
      

    //prep grid


      pathfinding = new Pathfinding(width, height, cellsize, showGizmos);

      //lets get some nodes to false
      pathfinding.GetGrid().GetGridObject(0, 3).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(1, 3).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(2, 3).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(2, 3).isWalkable = false;

      pathfinding.GetGrid().GetGridObject(0, 5).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(1, 5).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(2, 5).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(3, 5).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(4, 5).isWalkable = false;
      pathfinding.GetGrid().GetGridObject(5, 5).isWalkable = false;

      pathfinding.GetGrid().drawDebugGrid();

      pathfinding.GetGrid().OnGridValueChanged += (object sender, Mlf.Grid.Grid<PathNode>.OnGridValueChangedEventArgs e) =>
      {
        updateVisual = true;
      };


    }

    void BtnUpdateGrid(){
      Debug.Log ("You have clicked the button!");
    }


    void Update()
    {
      if(Input.GetMouseButtonDown(0))
      {
        Vector3 mouseWroldPosition = UtilsInput.GetMouseWorldPosition();
        pathfinding.GetGrid().GetGridPosition(mouseWroldPosition, out int x, out int y);

        List<PathNode> path = pathfinding.FindPath(0, 0, x, y);

        if (path != null)
        {
          for (int i = 0; i < path.Count - 1; i++)
          { 
            Debug.DrawLine( new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f,
                            new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f,
                            Color.white, 100f);
          }
        }
      }



      if (Input.GetMouseButtonDown(1))
      {
        Debug.Log("Mouse1 Clickded");
        Vector3 mouseWroldPosition = UtilsInput.GetMouseWorldPosition();
        pathfinding.GetGrid().GetGridPosition(mouseWroldPosition, out int x, out int y);
        PathNode pn = pathfinding.GetGrid().GetGridObject(x, y);
        pn.isWalkable = !pn.isWalkable;
        Debug.Log("Changed node: " + pn.isWalkable);
      }
    }

    public void  drawGridGizmos() {
      //pathfinding.GetGrid().drawDebugGrid();
    }


    private void OnDrawGizmos() {
      
    }
/*
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
*/


    private void LastUpdate()
    {
      if (updateVisual)
      {
        updateVisual = false;
        pathfinding.GetGrid().drawDebugGrid();
      }
    }
  }

}
