
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine {

  public class NpcWander : BasicSm
  {
    protected override void Start()
    {
      base.Start();
      setStates();
      this.SetState(StateNames.IdleRandomTime, false);
    }


    protected void setStates()
    {
      this.AddState(StateNames.IdleRandomTime, 
                    new IdleForRandomTimeState<BasicSm>(this));
      this.AddState(StateNames.Wander, 
                    new WanderState<BasicSm>(this));
    }

  }
}