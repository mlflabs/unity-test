using System;
using Mlf.Grid;

namespace Mlf.TileSystem.Grid
{
  public class TileNode
  {

    public Grid<TileNode> grid;
    public int x;
    public int y;

    //Path finding
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public TileNode cameFromNode;


    public TileNode(Grid<TileNode> grid, int x, int y, bool isWalkable = true)
    {
      this.grid = grid;
      this.x = x;
      this.y = y;
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