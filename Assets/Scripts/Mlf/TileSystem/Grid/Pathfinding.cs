using System;
using System.Collections.Generic;
using Mlf.Grid;
using Mlf.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mlf.TileSystem.Grid
{
  public class Pathfinder
  {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public Grid<TileNode> grid;
    //public List<TileNode> openList;
    //public List<TileNode> closedList;

    public Pathfinder(int width, int height, float cellsize) {
      grid = new Grid<TileNode>(width, 
                                height, 
                                1f, 
                                Vector3.zero, 
                                (Grid<TileNode> g, int x, int y) => new TileNode(g, x, y), 
                                true);
    }

    public List<TileNode> FindPath(int startX, int startY, int endX, int endY)
    {
      List<TileNode> openList;
      List<TileNode> closedList;


      TileNode startNode = grid.GetGridObject(startX, startY);
      TileNode endNode = grid.GetGridObject(endX, endY);
      openList = new List<TileNode> { startNode };
      closedList = new List<TileNode>();

      for(int x = 0; x < grid.Width; x ++)
      {
        for(int y = 0; y < grid.Height; y ++)
        {
          TileNode pathNode = grid.GetGridObject(x, y);
          pathNode.gCost = int.MaxValue;
          pathNode.CalculateFCost();
          pathNode.cameFromNode = null;
        }
      }

      startNode.gCost = 0;
      startNode.hCost = CalculateDistanceCost(startNode, endNode);
      startNode.CalculateFCost();

      while (openList.Count > 0)
      {
        TileNode currentNode = GetLowestFCostNode(openList);

        if(currentNode == endNode)
        {
          return CalculatedPath(endNode);
        }

        openList.Remove(currentNode);
        closedList.Add(currentNode);

        foreach (TileNode neighbourNode in GetNeighbourList(currentNode))
        {
          if(closedList.Contains(neighbourNode)) continue;
          
          if (neighbourNode.isWalkable == false)
          {
            Debug.Log($"Testing node at: {neighbourNode.x}, {neighbourNode.y}, walkable: {neighbourNode.isWalkable}");
            closedList.Add(neighbourNode);
            continue;
          }

          int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
          if(tentativeGCost < neighbourNode.gCost)
          {
            neighbourNode.cameFromNode = currentNode;
            neighbourNode.gCost = tentativeGCost;
            neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
            neighbourNode.CalculateFCost();

            if (!openList.Contains(neighbourNode))
            {
              openList.Add(neighbourNode);
            }
          }
        }
      }

      // out of nodes on the open list, no path found
      return null;

    }

    private List<TileNode> GetNeighbourList(TileNode currentNode)
    {
      List<TileNode> neighbourList = new List<TileNode>();

      if(currentNode.x - 1 >= 0)
      {
        neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y));
        if (currentNode.y > 0) neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y - 1));
        if (currentNode.y + 1  < grid.Height) neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y + 1));

      }
      if (currentNode.x + 1 < grid.Width)
      {
        neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y));
        if (currentNode.y > 0) neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y - 1));
        if (currentNode.y + 1 < grid.Height) neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y + 1));
      }

      if (currentNode.y > 0) neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y - 1));
      if (currentNode.y + 1 <  grid.Height) neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y + 1));

      return neighbourList;

    }

    private List<TileNode> CalculatedPath(TileNode endNode)
    {
      List<TileNode> path = new List<TileNode>();
      path.Add(endNode);
      TileNode currentNode = endNode;
      while(currentNode.cameFromNode != null)
      {
        path.Add(currentNode.cameFromNode);
        currentNode = currentNode.cameFromNode;
      }

      path.Reverse();
      return path;
    }

    private int CalculateDistanceCost(TileNode a, TileNode b)
    {
      int xDistance = Mathf.Abs(a.x - b.x);
      int yDistance = Mathf.Abs(a.y - b.y);
      int remaining = Mathf.Abs(xDistance - yDistance);
      return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private TileNode GetLowestFCostNode(List<TileNode> pathNodeList)
    {
      TileNode lowestFCostNode = pathNodeList[0];
      for(int i =  1; i <pathNodeList.Count; i ++)
      {
        if(pathNodeList[i].fCost < lowestFCostNode.fCost)
        {
          lowestFCostNode = pathNodeList[i];
        }
      }
      return lowestFCostNode;
    }

    // THIS SHOULD BE IN ITS OWN MONOBEHAVIOUR PATHFINDING VISIAL... just pass grid into it
    // use later to draw a visual
    public void drawNodeVisual()
    {
      UtilsMesh.CreateEmptyMeshArrays(grid.Width * grid.Height,
                                      out Vector3[] vertices,
                                      out Vector2[] uvs,
                                      out int[] triangles);

      for(int x = 0; x < grid.Width; x++)
      {
        for(int y = 0; y < grid.Height; y++)
        {
          int index = x * grid.Height + y;

          Vector3 quadsize = new Vector3(1, 1) * grid.CellSize;

          TileNode pathNode = grid.GetGridObject(x, y);

          if(pathNode.isWalkable)
          {
            quadsize = Vector3.zero;
          }

          //UtilsMesh.AddToMeshArrays(vertices, uvs, triangles, index, grid.GetWorldPosition(x,y) + quadsize * 0.5f,
                                  //  0f, quadsize...


        }
      }

     //mesh.vertices = vertices;
     //mesh.uv = uv;
     //mesh.triangles = triangles;
    }





  }

}
