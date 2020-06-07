
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Actions;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine {

  public class PlayerSm : BasicSm
  {

  
    protected override void Start()
    {
      base.Start();

      setStates();
      this.InitState(StateNames.Idle);

      addActions();
    }


    private void setStates()
    {
      this.AddState(StateNames.Idle, new IdleState<BasicSm>(this));
      this.AddState(StateNames.Move, new WalkingState<BasicSm>(this));
      

    }

    protected void addActions() {
      //FixedTimeIntervalUpdate<BasicSm> intervalAction = 
      //  new FixedTimeIntervalUpdate<BasicSm>(this, 5f,new List<BaseAction<BasicSm>> {
      //    new UpdateUserLifeStats<BasicSm>(this),
      //  });

      WaypointToJoystickDirectionChange<BasicSm> joystickAnimations = 
        new WaypointToJoystickDirectionChange<BasicSm>(this);

      this.preStateActions.Add(joystickAnimations);
      //this.preStateActions.Add(intervalAction);

      //fixed actions
      MoveToWaypoint<BasicSm> moveToWaypoint = 
        new MoveToWaypoint<BasicSm>(this);

      MoveToWaypointDrawGizmos<BasicSm> moveToWaypointDrawGizmos = 
        new MoveToWaypointDrawGizmos<BasicSm>(this);

      this.postStateFixedActions.Add(moveToWaypoint);
      this.postStateFixedActions.Add(moveToWaypointDrawGizmos);

    }

    private void OnApplicationQuit() {
      inventory.items.Clear();  
    }

  }
}