using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Actions;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States {


  // Use this to go to single destination
  public class WanderState<T> : BaseState<T> where T: IBasicSm
  {

    private  float bufferDistanceSqr;
    private  float wanderRadius;

    private bool searching;
    public WanderState(T sm) : base(sm) { 
      stateName = StateNames.Wander;
      baseStateName = StateNames.Move;
      bufferDistanceSqr = sm.motionData.targetDistanceBuffer * sm.motionData.targetDistanceBuffer;
    }

    

    public override void Start() {
      //set a new path in random location
      Vector2 point = (Random.insideUnitCircle * 
                      sm.motionData.randomLocationRomingRadius) +
                      sm.rb.position;
      searching = true;
      sm.seeker.StartPath(sm.rb.position, point, onPathComplete);
    }

    private void onPathComplete(Path p) {
      sm.motionData.setPath(p);
      searching = false;
    }
    
    public override void Update () {
      


      //wait till we got our path
      if(searching) return;

      float distance = (sm.motionData.currentWaypoint - sm.rb.position).sqrMagnitude;

      

      if(distance < bufferDistanceSqr) {
        //do we have a next waypoint
        if(sm.motionData.pathWaypoints != null && 
           sm.motionData.pathWaypoints.Count > 1)
        {
          sm.motionData.pathWaypoints.RemoveAt(0);
          sm.motionData.currentWaypoint = sm.motionData.pathWaypoints[0];
        }
        else {
          //finished, next state
          sm.motionData.clearOldPathData();
          sm.SetState(StateNames.IdleRandomTime, true);
        }

      }

    }

    public override void FixedUpdate() {
      //MoveByJoystickDirection.tick(this.sm, Time.deltaTime);
      //MoveToWaypoint.tick(sm, Time.deltaTime);
      
      //sm.rb.MovePosition(sm.rb.position + sm.joyStickChange.normalized * 
      //                   sm.speed * deltaTime);
    }

  }
}