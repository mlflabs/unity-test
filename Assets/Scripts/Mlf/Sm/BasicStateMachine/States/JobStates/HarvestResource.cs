
using Mlf.Gm;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States.StatsRecover {

  public class HarvestResource<T>: BaseState<T>
    where T : BasicSm {


    private float waitTime = 1f;
  
    private float currentTime = 0;
    public HarvestResource(T sm, string stateName) : base(sm) { 
      this.stateName = stateName;
      baseStateName = StateNames.Idle;
      
    }

    public override void Start() {
      sm.motionData.currentWaypoint = Vector2.zero;
      sm.motionData.destination = Vector2.zero;

      if(sm.jobData.harvestTarget == null) {
        Debug.LogError("HarvestResourceState but no harvestTarget given");
      }
    }

    public override void Update () {

      currentTime += Time.deltaTime;

      if(waitTime > currentTime) return;

      currentTime = 0;
        

      //see if we are full
      if(sm.inventory.MaxInventoryReached()){
          Debug.Log("Harvesting - We are full find storage to depoist");
          //clear our reservation to this item
          GameResourceManager.instance
            .removeHarvestItemReservation(sm.jobData.harvestTarget);
          sm.setNextState();
          return;
      }

      //see if the harvest item is empty
      if(sm.jobData.harvestTarget.isEmpty()){
          Debug.Log("Harvesting - Target is empty, find a new one");
          sm.jobData.harvestTarget.destroyItem();
          
          JobData data = sm.jobData;
          data.harvestTarget = null;
          sm.jobData = data;
          sm.setNextState();
          return;
      }

      Debug.Log("Harvesting in progress");
      sm.inventory.AddItem(sm.jobData.harvestTarget.inventory.removeRandomItem());
        
    }


    
  }
}