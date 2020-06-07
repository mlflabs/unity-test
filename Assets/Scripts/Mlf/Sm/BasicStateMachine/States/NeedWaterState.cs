using Mlf.Gm;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Actions;
using Mlf.Sm.BasicStateMachine.Data;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States {


  // Use this to go to single destination
  public class NeedWaterState<T> : BaseState<T> where T: IBasicSm
  {
    
    private  float bufferDistanceSqr;
    private  float wanderRadius;

    private string fulfillPostState;

    private GameObject targetObject;

    private bool searching;
    public NeedWaterState(T sm, string fulfillPostState) : base(sm) { 
      stateName = StateNames.Thirsty;
      baseStateName = StateNames.Move;
      this.fulfillPostState = fulfillPostState;
      bufferDistanceSqr = sm.motionData.targetDistanceBuffer * sm.motionData.targetDistanceBuffer;
    }
    
    //We are hungry , lets start searching for grass
    public override void Start() {
      //set a new path in random location
      //find the closes food location
      float distance = float.MaxValue;
      targetObject = GameManager.instance.waterLocations[0];
      for(int i = 0; i < GameManager.instance.waterLocations.Length; i ++) {
        float newDistance = ( this.sm.rb.position - 
                              (Vector2) GameManager.instance.waterLocations[i].transform.position).sqrMagnitude;
        if(newDistance < distance){
          distance = newDistance;
          Debug.Log("NEW DISTANCE::::: "+ distance +"   "+newDistance+"   "+i);
          targetObject = GameManager.instance.waterLocations[i];
        }
      }

      searching = true;
      sm.seeker.StartPath(sm.rb.position, targetObject.transform.position, onPathComplete);
    }

    private void onPathComplete(Path p) {
      sm.motionData.setPath(p);
      searching = false;

    }
    
    public override void Update () {
      


      //wait till we got our path
      if(searching) return;

      float distance = (sm.motionData.currentWaypoint - sm.rb.position).sqrMagnitude;

      Debug.DrawLine(sm.rb.position, sm.motionData.currentWaypoint, Color.cyan, Time.deltaTime);
      Debug.DrawLine( sm.rb.position, 
                      sm.motionData.pathWaypoints[sm.motionData.pathWaypoints.Count-1], 
                      Color.green, Time.deltaTime);

      if(distance < bufferDistanceSqr) {
        //do we have a next waypoint
        if(sm.motionData.pathWaypoints != null && 
           sm.motionData.pathWaypoints.Count > 1)
        {
          sm.motionData.pathWaypoints.RemoveAt(0);
          sm.motionData.currentWaypoint = sm.motionData.pathWaypoints[0];
          //also test if we are near enough to the target
          if(((Vector2) targetObject.transform.position - sm.rb.position).sqrMagnitude < bufferDistanceSqr){
            //we are near 
            sm.statsData = StatsDataModifiers.drink(sm.statsData);
            sm.SetState(fulfillPostState, false);
          }
        }
        else {
          if(((Vector2) targetObject.transform.position - sm.rb.position).sqrMagnitude < bufferDistanceSqr){
            //we are near 
            sm.statsData = StatsDataModifiers.drink(sm.statsData);
            sm.SetState(fulfillPostState, false);
          }
          else {
          //finished, next state
          sm.motionData.clearOldPathData();
          sm.SetState(StateNames.IdleRandomTime, true);
          }
         
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