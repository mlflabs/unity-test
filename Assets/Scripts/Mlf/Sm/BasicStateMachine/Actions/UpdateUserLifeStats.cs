using System;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.Actions {

  public class UpdateUserLifeStats<T> : BaseAction<T> where T: BasicSm
  {

    private float lastUpdateTime;
    public UpdateUserLifeStats(T sm): base(sm)
    {
      lastUpdateTime = Time.fixedTime;
    }

    public override bool tick() {
      sm.statsData = StatsDataModifiers.updateStats(sm.statsData);

      //check if we are in need of something
      if(sm.statsData.food < sm.statsData.foodMax / 10){
        //hungry
        sm.SetState(StateNames.Hungry);
        return false;
      }

      if(sm.statsData.water < sm.statsData.waterMax / 10){
        //hungry
        sm.SetState(StateNames.Thirsty);
        return false;
      }



      return true;
       
    }
  }
}
