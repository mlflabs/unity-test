using System;
using UnityEngine;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using Pathfinding;

namespace Mlf.Sm.BasicStateMachine {

  public interface IBasicSm : IBaseSm
  {
    Animator animator {get; set;}
    Rigidbody2D rb {get;set;}
    string nextState {get; set;}
    MotionData motionData {get; set;}
    BaseData baseData {get; set;}
    StatsData statsData {get; set;}
    Seeker seeker {get; set;}
  }  
}