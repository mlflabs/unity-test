using System;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.Actions {

  public class UpdateAnimations<T> : BaseAction<T> where T: BasicSm
  {


    public UpdateAnimations(T sm): base(sm)
    {
    }

    public override bool tick() {
      sm.animator.SetFloat("x", sm.motionData.joyStickChange.x);
      sm.animator.SetFloat("y", sm.motionData.joyStickChange.y);

// this doesn't work, since even when stoped we will keep the
//joystick values for idle direciton
      if(sm.motionData.joyStickChange == Vector2.zero) {
        sm.animator.SetFloat("speed", 0);
      }
      else {
        sm.animator.SetFloat("speed", 1);
      }
      

   


      return true;
       
    }
  }
}
