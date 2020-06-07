
using System.Collections.Generic;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Actions;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine {

  public class NpcWanderWaterFood : BasicSm
  {
    protected override void Start()
    {
      base.Start();
      setStates();

      // setup our data
      this.statsData = StatsDataModifiers.getBasicData();

      this.InitState(StateNames.IdleRandomTime);
      
      addActions();
    }

    protected void addActions() {
      FixedTimeIntervalUpdate<BasicSm> intervalAction = 
        new FixedTimeIntervalUpdate<BasicSm>(this, 5f,new List<BaseAction<BasicSm>> {
          new UpdateUserLifeStats<BasicSm>(this),
        });
      WaypointToJoystickDirectionChange<BasicSm> joystickAnimations = 
        new WaypointToJoystickDirectionChange<BasicSm>(this);

      this.preStateActions.Add(joystickAnimations);
      this.preStateActions.Add(intervalAction);

      //fixed actions
      MoveToWaypoint<BasicSm> moveToWaypoint = 
        new MoveToWaypoint<BasicSm>(this);

      this.postStateFixedActions.Add(moveToWaypoint);

    }

    protected void setStates()
    {
      this.AddState(StateNames.IdleRandomTime, 
                    new IdleForRandomTimeState<BasicSm>(this));
      this.AddState(StateNames.Wander, 
                    new WanderState<BasicSm>(this));
      this.AddState(StateNames.Hungry, new NeedGrassState<BasicSm>(this, StateNames.IdleRandomTime));
      this.AddState(StateNames.Thirsty, new NeedWaterState<BasicSm>(this, StateNames.IdleRandomTime));
    

    }

  }
}