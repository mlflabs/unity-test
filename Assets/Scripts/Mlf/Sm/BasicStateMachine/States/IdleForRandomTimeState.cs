
using Mlf.Sm.Base;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States {

  public class IdleForRandomTimeState<T>: BaseState<T>
    where T : BasicSm {


   

    private float waitTime = 0;
    private float currentTime = 0;
    public IdleForRandomTimeState(T sm) : base(sm) { 
      stateName = StateNames.IdleRandomTime;
      baseStateName = StateNames.Idle;
    }

    public override void Start() {
      sm.motionData.currentWaypoint = Vector2.zero;
    }

    public override void Update () {
      if(waitTime == 0) {
        currentTime = 0;
        waitTime =  Random.Range(sm.motionData.idleRandomTimeMinSeconds, 
                                 sm.motionData.idleRandomRimeMaxSeconds);
      }

      currentTime += Time.deltaTime;
      
      if(currentTime > waitTime) {
        waitTime = 0;
        currentTime = 0;
        //change state;
        sm.setNextState();
        
      }
    }
  }
}