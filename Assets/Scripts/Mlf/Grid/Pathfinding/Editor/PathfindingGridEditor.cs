
using UnityEngine;
using UnityEditor;
using Mlf.Grid.Pathfinding;

[CustomEditor(typeof(PathfindingGrid))]
public class PathfindingGridEditor : Editor {

  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    PathfindingGrid grid = (PathfindingGrid)target;

    if(GUILayout.Button("Update Grid")) {
      Debug.Log("Gui pressed.....");
      grid.drawGridGizmos();
    }
  }

}