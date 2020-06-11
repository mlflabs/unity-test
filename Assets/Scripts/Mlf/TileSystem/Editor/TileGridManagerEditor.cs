
using UnityEngine;
using UnityEditor;
using Mlf.Grid.Pathfinding;
using Mlf.TileSystem;
using UnityEngine.Tilemaps;
using Mlf.TileSystem.Tiles;

namespace Mlf.TileSystem {
  
  [CustomEditor(typeof(TileGridManager))]
  public class TileGridManagerEditor : Editor {

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      TileGridManager manager = (TileGridManager) target;

      if(GUILayout.Button("Scan Tilemaps")) {
        Debug.Log("Scan Strted");
        scanAllTilemapGrids();
      }
    }


    private void scanAllTilemapGrids() {
      TilemapComp[] maps = FindObjectsOfType<TilemapComp>();

      foreach(TilemapComp map in maps) {
        Debug.Log("****************** Tilemap: " + map.name);
        Tilemap tilemap = map.GetComponent<Tilemap>();

        if(!tilemap) continue;

        foreach (var position in tilemap.cellBounds.allPositionsWithin) {
            if (!tilemap.HasTile(position)) {
                Debug.Log("Empty::: " + position);
                continue;
            }

            PathTerrainTile tile = (PathTerrainTile) tilemap.GetTile(position);

            
            Debug.Log("Found tile: " + position);
                       
            Debug.Log(tile.name + " Walkable: " + tile.pathData.isWalkable);
            Vector3 tilePosition = tilemap.CellToWorld(position);
            Debug.Log("World Position::: " + tilePosition);
            Gizmos.DrawLine(tilePosition, 
                          new Vector3(tilePosition.x +1,tilePosition.y +1));
            
        }

        // draw bounds of tilemap
        Vector3 boundsStart = tilemap.CellToWorld(new Vector3Int(
            tilemap.cellBounds.xMin, tilemap.cellBounds.yMin, 0));
        Vector3 boundsEnd = tilemap.CellToWorld(new Vector3Int(
            tilemap.cellBounds.xMax-1, tilemap.cellBounds.yMax -1, 0));
        boundsEnd.y += tilemap.cellSize.y;
        Debug.DrawLine(boundsStart, boundsEnd, Color.red);
      }

    }

  }

  public static class TileTypes {
    public static string test = "default";
  }
}

