using Mlf.Gm;
using Mlf.InventorySystem;
using Mlf.InventorySystem.Items;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Actions;
using Mlf.Sm.BasicStateMachine.Data;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States {

  public class HarvestControlState<T> : BaseState<T> where T: BasicSm
  {

    public HarvestTypes harvestType;
    public ItemType storageType;
  

    private  float bufferDistanceSqr;

    private GameObject harvestTarget;


    private bool searching;
    public HarvestControlState(T sm, string harvestTypeState, ItemType storageType, HarvestTypes harvestType) : base(sm) { 
      Debug.Log("****************: harvest default state:::: " + harvestTypeState);
      this.storageType = storageType;
      this.harvestType = harvestType;
    

      stateName = harvestTypeState;
      baseStateName = StateNames.Idle;
      bufferDistanceSqr = sm.motionData.targetDistanceBuffer * sm.motionData.targetDistanceBuffer;
    }

    

    public override void Start() {
      Debug.Log("**************** Harvest Control State");
      JobData jd; // use as placeholder 

      //Are We Full
      if(sm.inventory.MaxInventoryReached()) {
        Debug.Log("Max Capacity Reached");
        //get closes storage poing, dump the items
        jd = sm.jobData;

        jd.storageTarget = GameResourceManager.instance
               .getClosestStoragePlace(sm.transform.position, storageType, true);
        
       
        
        if(jd.storageTarget == null) {
          Debug.Log("We don't have a suitable storage target place");
          //show we don't have storage target
          sm.nextState = stateName;
          sm.SetState(StateNames.IdleRandomTime);
          return;
        }
        
        Debug.Log("Storage Destination found, moving to it");
        sm.jobData = jd;
        sm.motionData.destination = sm.jobData.storageTarget.transform.position;
        sm.nextState = StateNames.DepositItemsToStorage;
        sm.SetState(StateNames.MoveToDestination);
        return;
      }


      //We are not full, so lets find target
      jd = sm.jobData;
      jd.harvestTarget = GameResourceManager.instance
               .getClosestHarvestItem(harvestType, this.sm);

      if(jd.harvestTarget == null) {
        Debug.Log("Harvest Target Null, npc: " + sm.baseData.name);
        sm.nextState = stateName;
        sm.SetState(StateNames.IdleRandomTime);
        return;
      }

      Debug.Log(jd.harvestTarget);
      sm.jobData = jd;
      sm.nextState = StateNames.Job_HarvestResource;
      sm.motionData.destination = sm.jobData.harvestTarget.transform.position;
      sm.SetState(StateNames.MoveToDestination);
      return;

    }

    
    
    public override void Update () {
  
    }

    public override void FixedUpdate() {
     
    }



  }
}