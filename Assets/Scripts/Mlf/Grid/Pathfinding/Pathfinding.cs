﻿using System;
using System.Collections.Generic;
using Mlf.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mlf.Grid.Pathfinding
{
  public class Pathfinding
  {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height, float cellsize, bool showGizmos) {
      grid = new Grid<PathNode>(width, 
                                height, 
                                1f, 
                                Vector3.zero, 
                                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), true);
    }

    public Grid<PathNode> GetGrid()
    {
      return grid;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
      PathNode startNode = grid.GetGridObject(startX, startY);
      PathNode endNode = grid.GetGridObject(endX, endY);
      openList = new List<PathNode> { startNode };
      closedList = new List<PathNode>();

      for(int x = 0; x < grid.Width; x ++)
      {
        for(int y = 0; y < grid.Height; y ++)
        {
          PathNode pathNode = grid.GetGridObject(x, y);
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
        PathNode currentNode = GetLowestFCostNode(openList);

        if(currentNode == endNode)
        {
          return CalculatedPath(endNode);
        }

        openList.Remove(currentNode);
        closedList.Add(currentNode);

        foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
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

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
      List<PathNode> neighbourList = new List<PathNode>();

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

    private List<PathNode> CalculatedPath(PathNode endNode)
    {
      List<PathNode> path = new List<PathNode>();
      path.Add(endNode);
      PathNode currentNode = endNode;
      while(currentNode.cameFromNode != null)
      {
        path.Add(currentNode.cameFromNode);
        currentNode = currentNode.cameFromNode;
      }

      path.Reverse();
      return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
      int xDistance = Mathf.Abs(a.x - b.x);
      int yDistance = Mathf.Abs(a.y - b.y);
      int remaining = Mathf.Abs(xDistance - yDistance);
      return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
      PathNode lowestFCostNode = pathNodeList[0];
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

          PathNode pathNode = grid.GetGridObject(x, y);

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
