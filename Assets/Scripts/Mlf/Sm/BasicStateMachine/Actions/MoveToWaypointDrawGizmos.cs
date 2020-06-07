using System;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.States;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.Actions {


  public class MoveToWaypointDrawGizmos<T> : BaseAction<T> where T: BasicSm
  {

    public MoveToWaypointDrawGizmos(T sm): base(sm)
    {
    }

    public override bool tick() {
      
      if(sm.motionData.currentWaypoint.x == -9999) return true;;
      
      Debug.DrawLine(sm.transform.position, sm.motionData.currentWaypoint, Color.cyan, Time.deltaTime);

      if(sm.motionData.pathWaypoints == null) return true;
      //Debug.Log(sm.motionData.pathWaypoints);
      Debug.DrawLine( sm.transform.position, 
                      sm.motionData.pathWaypoints[sm.motionData.pathWaypoints.Count-1], 
                      Color.green, Time.deltaTime);

     
      return true;
    
    }
  }  
}
