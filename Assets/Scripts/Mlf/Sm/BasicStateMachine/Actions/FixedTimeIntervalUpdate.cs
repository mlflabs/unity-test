using System;
using System.Collections.Generic;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.States;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.Actions {

  public class FixedTimeIntervalUpdate<T> : BaseAction<T> where T: BasicSm
  {

    public List<BaseAction<T>> actions; 
         
    
    private float nextTick;
    private float timeInterval;
    public FixedTimeIntervalUpdate( T sm,
                                    float timeInterval, 
                                    List<BaseAction<T>> actions): base(sm) {
    
      this.actions = actions;
      this.timeInterval = timeInterval;
      nextTick = Time.fixedTime + timeInterval;
    
    }

    public override bool tick() {
      if(Time.fixedTime < nextTick) return true;

      nextTick = Time.fixedTime + timeInterval;
      bool next;
      foreach(var a in actions){
        next = a.tick();
        if(!next){
          break;
        };
      }

      return true;       
    }
  }
}
