using System;
using Mlf.Grid;

namespace Mlf.Grid.Tilemap
{
  public class TileNode
  {

    private Grid<TileNode> grid;
    private int _x;
    private int _y;

    public TileNode cameFromNode;

    public int x { get => _x;  }
    public int y { get => _y; }

    public TileNode(Grid<TileNode> grid, int x, int y)
    {
      this.grid = grid;
      _x = x;
      _y = y;
    }

    public override string ToString()
    {
      return "(" + x + ", " + y + ") ";
    }
  }
}