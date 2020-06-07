using System;
using UnityEngine;
using Pathfinding;
using Mlf.InventorySystem;
using Mlf.InventorySystem.GameObjects;

namespace Mlf.Sm.BasicStateMachine.Data {

  [Serializable] 
  public struct JobData {
    public string currentJob;
    public BaseInventoryComp storageTarget;
    public HarvestItemComp harvestTarget;
  }

  public static  class JobDataModifiers {

    public static JobData getBasicNpcData(string currentJob) {
      return new JobData {
        currentJob = currentJob,
        storageTarget = null,
        harvestTarget = null,
      };
    }

    public static JobData setStorageTarget(BaseInventoryComp storageTarget, JobData data) {
      data.storageTarget = storageTarget;
      return data;
    }

    public static JobData setHarvestTarget(HarvestItemComp harvestTarget, JobData data) {
      data.harvestTarget = harvestTarget;
      return data;
    }



  }
}