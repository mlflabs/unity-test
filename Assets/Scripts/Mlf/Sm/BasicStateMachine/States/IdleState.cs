using System;
using Mlf.Sm.Base;
using UnityEngine;
using Mlf.Sm.BasicStateMachine;

namespace Mlf.Sm.BasicStateMachine.States {

  public class IdleState<T>: BaseState<T>
    where T : IBasicSm {

    
    public override void Start() {
      sm.motionData.currentWaypoint = Vector2.zero;
    }

    public IdleState(T sm) : base(sm) {
      stateName = StateNames.Idle;
      baseStateName = StateNames.Idle;
    }

    public override void Update () {
      //GetUserAllInputs.tick(this.sm, Time.deltaTime);
      if(sm.motionData.destination != null && sm.motionData.destination.x != -9999)
      {
        sm.SetState(StateNames.Move, true);
      }
    }
  }
}