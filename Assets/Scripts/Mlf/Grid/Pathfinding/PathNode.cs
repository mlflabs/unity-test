using System;
using Mlf.Grid;

namespace Mlf.Grid.Pathfinding
{
  public class PathNode
  {

    private Grid<PathNode> grid;
    private int _x;
    private int _y;



    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;

    public int x { get => _x;  }
    public int y { get => _y; }

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
      this.grid = grid;
      _x = x;
      _y = y;
      isWalkable = true;
    }

    public override string ToString()
    {
      return "(" + x + ", " + y + ") " + isWalkable;
    }

    public void CalculateFCost()
    {
      fCost = gCost + hCost;
    }
  }
}