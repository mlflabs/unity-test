using Mlf.Sm.Base;
using UnityEngine;
using Pathfinding;

namespace Mlf.Sm.BasicStateMachine.States {


  // Use this to go to single destination
  public class WalkingState<T> : BaseState<T> where T: BasicSm
  {

    private bool searching;

    private Vector2 currentDestination;

    private  float bufferDistanceSqr;
    public WalkingState(T sm) : base(sm) { 
      stateName = StateNames.Move;
      baseStateName = StateNames.Move;
      bufferDistanceSqr = sm.motionData.targetDistanceBuffer * sm.motionData.targetDistanceBuffer;
    }
    public override void Start() {
      if(sm.motionData.destination != Vector2.zero){
        findPath();
      }
    }

    private void findPath() {
      Debug.Log("Finding Path Checks");
      if(sm.motionData.destination == null) return;

      //see if we are really close to destination 
      float distance = (sm.motionData.destination - 
                        (Vector2) sm.transform.position).sqrMagnitude;
        
      if (distance < bufferDistanceSqr){
        Debug.Log("Already close to destination");
        nextState();
        return;
      }

      Debug.Log("Checks good, lets find path");
      searching = true;
      currentDestination = sm.motionData.destination;

      sm.seeker.StartPath(sm.rb.position, sm.motionData.destination, onPathComplete);
    }

    private void onPathComplete(Path p) {
      sm.motionData.setPath(p);
      searching = false;
    }
    
    public override void Update ()
    {

      //see if we have a new destination
      if (currentDestination != sm.motionData.destination)
      {
        findPath();
        return;
      }

      if (searching)
      {
        return;
      }
      
      float distance = (sm.motionData.currentWaypoint - (Vector2) sm.transform.position).sqrMagnitude;

      if (distance < bufferDistanceSqr)
      {
        //do we have a next waypoint
        if (sm.motionData.pathWaypoints != null &&
           sm.motionData.pathWaypoints.Count > 1)
        {
          sm.motionData.pathWaypoints.RemoveAt(0);
          sm.motionData.currentWaypoint = sm.motionData.pathWaypoints[0];
        }
        else
        {
          //finished, next state
          nextState();
          
        }

      }

      //WaypointToJoystickDirectionChange.tick(sm);
    }

    public void nextState() {
      sm.motionData.pathWaypoints = null;
      sm.motionData.currentWaypoint.x = -9999;
      sm.motionData.destination.x = -9999;
      sm.setNextState();
    }

    public override void FixedUpdate() {
      //MoveByJoystickDirection.tick(this.sm, Time.deltaTime);
      //MoveToWaypoint.tick(sm, Time.deltaTime);
      
      //sm.rb.MovePosition(sm.rb.position + sm.joyStickChange.normalized * 
      //                   sm.speed * deltaTime);
    }

  }
}