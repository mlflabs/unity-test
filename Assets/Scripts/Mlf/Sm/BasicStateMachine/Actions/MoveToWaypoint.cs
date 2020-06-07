using System;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.States;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.Actions {


  public class MoveToWaypoint<T> : BaseAction<T> where T: BasicSm
  {

    public MoveToWaypoint(T sm): base(sm)
    {
    }

    public override bool tick() {
      //Debug.LogWarning(sm.currentState.baseStateName);
      if(this.sm.currentState.baseStateName != StateNames.Move) return true;
      //TODO: look at the ground we are over, maybe change the speed
      //sm.rb.MovePosition(sm.rb.position + sm.joyStickChange.normalized * sm.speed * deltaTime);
      //sm.rb.position =  Vector2.MoveTowards(sm.rb.position, sm.nextWaypoint, sm.speed * deltaTime);
      //Debug.Log(sm.motionData.joyStickChange);
      //Debug.Log("1: " + ((Vector2) sm.transform.position + sm.motionData.joyStickChange * sm.motionData.speed * Time.deltaTime));
      sm.transform.position = ((Vector2) sm.transform.position + sm.motionData.joyStickChange * sm.motionData.speed * Time.deltaTime);
      //Debug.Log("2: " + sm.transform.position);
    
      return true;
    
    }
  }  
}
