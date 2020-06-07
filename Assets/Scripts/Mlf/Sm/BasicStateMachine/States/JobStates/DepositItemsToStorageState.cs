
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine.States.StatsRecover {

  public class DepositItemsToStorageState<T>: BaseState<T>
    where T : BasicSm {


    private float waitTime = 0.3f;
  
    private float currentTime = 0;
    public DepositItemsToStorageState(T sm) : base(sm) { 
      stateName = StateNames.Job_DropItemsToStorage;
      baseStateName = StateNames.Idle;
      
    }

    public override void Start() {
      sm.motionData.currentWaypoint = Vector2.zero;
      sm.motionData.destination = Vector2.zero;

      if(sm.jobData.storageTarget == null) {
        Debug.LogError("DropState but no storageTargetSelected");
      }
    }

    public override void FixedUpdate() {
      Debug.Log("Deposit State Tick");
      currentTime += Time.deltaTime;

      if(waitTime > currentTime) return;

      currentTime = 0;
        
      bool tookItem = false;

      //is storage full, if so wait a random time and try again
      if(sm.jobData.storageTarget.inventory.MaxInventoryReached()) {
        StorageCapacityMaxedOut();
        return;
      }

      for(int i = 0;  i < sm.inventory.items.Count; i++) {
        tookItem = sm.jobData.storageTarget.acceptItem(sm.inventory.items[i].item);
        Debug.Log("tookItem::: " + tookItem + "i: " + i);
        if(tookItem) {
          Debug.Log("Took one item");
          sm.inventory.removeOneItemAmountByIndex(i);
          return;
        }
          
      }

      //if we are here, we have given all possible items
      Debug.Log("Leaving Drop state for next State: " + sm.nextState);
      sm.setNextState();
     
    }

    private void StorageCapacityMaxedOut() {
      //here we could search for the next storage capacity place
      sm.nextState = StateNames.DepositItemsToStorage;
      sm.SetState(StateNames.IdleRandomTime);
    }
  }
}