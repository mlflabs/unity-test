
using System.Collections.Generic;
using Mlf.InventorySystem.Items;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Actions;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using Mlf.Sm.BasicStateMachine.States.StatsRecover;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine {

  public class PlayerWorker : BasicSm
  {
    
    protected override void Start()
    {
      base.Start();
      setStates();

      // setup our data
      this.statsData = StatsDataModifiers.getBasicData();
      this.jobData = JobDataModifiers.getBasicNpcData(StateNames.Job_HarvestGrassControl);


      this.InitState(StateNames.IdleRandomTime);
      
      addActions();
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

    protected void setStates()
    {
      this.AddState(StateNames.MoveToDestination, 
                    new WalkingState<BasicSm>(this));
      this.AddState(StateNames.DepositItemsToStorage, 
                    new DepositItemsToStorageState<BasicSm>(this));
      this.AddState(StateNames.StaminaRecovery, 
                    new StaminaRestRecoveryState<BasicSm>(this));
      this.AddState(StateNames.IdleRandomTime, 
                    new IdleForRandomTimeState<BasicSm>(this));
      this.AddState(StateNames.Job_HarvestResource,
                    new HarvestResource<BasicSm>(this, StateNames.Job_HarvestResource));
      this.AddState(StateNames.Job_HarvestGrassControl, 
                    new HarvestControlState<BasicSm>(
                      this, 
                      StateNames.Job_HarvestGrassControl, 
                      ItemType.Grass,
                      Gm.HarvestTypes.cutGrass));
      //this.AddState(StateNames.Hungry, new NeedGrassState<BasicSm>(this, StateNames.IdleRandomTime));
      //this.AddState(StateNames.Thirsty, new NeedWaterState<BasicSm>(this, StateNames.IdleRandomTime));
    

    }

    public override void setNextState() {
      if(this.nextState != null) {
        Debug.Log(this.nextState);
        SetState(this.nextState);
        this.nextState = null;
        return;
      }
      
      SetState(jobData.currentJob);
      return;
      
      //Debug.LogError("Next State Not Defined");
    }

  }
}