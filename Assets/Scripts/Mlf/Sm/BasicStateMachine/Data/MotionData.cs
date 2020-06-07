using System;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

namespace Mlf.Sm.BasicStateMachine.Data {

  [Serializable] public class MotionData {

    public Vector2 joyStickChange;
    public float speed = 2f;

    public Vector2 destination;
    public Vector2 currentWaypoint;
    public GameObject followObject;
    public List<Vector2> pathWaypoints {get; set;}

    public float targetDistanceBuffer = 1f;

    public float idleRandomTimeMinSeconds = 1f;
    public float idleRandomRimeMaxSeconds = 5f;
    public float randomLocationRomingRadius = 10f;



    public void setPath(Path p) {
      pathWaypoints = p.vectorPath.ConvertAll(x => new Vector2(x.x, x.y));

      if(pathWaypoints.Count > 0) //first element is the current position of object
        pathWaypoints.RemoveAt(0);
      
      currentWaypoint = pathWaypoints[0];
    }
    

    public void clearOldPathData() {
      pathWaypoints = null;
      currentWaypoint = Vector2.zero;
    }
  }
}