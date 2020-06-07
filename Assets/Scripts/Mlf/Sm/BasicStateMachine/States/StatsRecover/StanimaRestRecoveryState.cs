
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States.StatsRecover {

  public class StaminaRestRecoveryState<T>: BaseState<T>
    where T : BasicSm {


    private float waitTime = 1f;
  
    private float currentTime = 0;
    public StaminaRestRecoveryState(T sm) : base(sm) { 
      stateName = StateNames.StaminaRecovery;
      baseStateName = StateNames.Idle;
    }

    public override void Start() {
      sm.motionData.currentWaypoint = Vector2.zero;
    }

    public override void Update () {
      currentTime += Time.deltaTime;

      if(currentTime > waitTime) {
        currentTime = 0;
        
        sm.statsData = StatsDataModifiers.addStanima(sm.statsData.stanimaRecoverPerSec, sm.statsData);

        if(sm.statsData.stanima >= sm.statsData.stanimaMax)
          sm.setNextState();
      }
    }
  }
}