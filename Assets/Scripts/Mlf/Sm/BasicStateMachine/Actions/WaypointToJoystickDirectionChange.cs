using System;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.States;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.Actions {

  public class WaypointToJoystickDirectionChange<T> : BaseAction<T> where T: BasicSm
  {

    public WaypointToJoystickDirectionChange(T sm): base(sm)
    {
    }

    public override bool tick() {

      if(sm.motionData.currentWaypoint != Vector2.zero){
        sm.motionData.joyStickChange = (sm.motionData.currentWaypoint - sm.rb.position);
       
        if(sm.animator != null){
          sm.animator.SetFloat("x", sm.motionData.joyStickChange.x);
          sm.animator.SetFloat("y", sm.motionData.joyStickChange.y);
          sm.animator.SetInteger("action", 1);
          //sm.animator.SetBool("move", true);
        }

      }
      else {
        if(sm.animator != null)
          //sm.animator.SetBool("move", false);
          sm.animator.SetInteger("action", 0);
      }

      return true;
    }
  }  
}
