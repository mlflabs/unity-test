using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mlf.Grid.Tilemap
{
  public class TileGrid
  {

  
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<TileNode> _grid;
    public Grid<TileNode> grid {get=>_grid; set=>_grid = value;}
    private List<TileNode> openList;
    private List<TileNode> closedList;

    public TileGrid(int width, 
                    int height, 
                    float cellsize, 
                    Vector3 originPosition,
                    bool showGizmos) 
    {
      grid = new Grid<TileNode>(width, 
                                height, 
                                1f, 
                                originPosition, 
                                (Grid<TileNode> g, int x, int y) => new TileNode(g, x, y), true);
    }

    
  


  }

}
