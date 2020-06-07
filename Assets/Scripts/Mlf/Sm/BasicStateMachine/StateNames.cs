

using UnityEngine;

namespace Mlf.Sm.BasicStateMachine {

  public static class StateNames {
    public static string Idle = "Idle";
    public static string IdleRandomTime = "IdleRandomTime";
    public static string Move = "Move";
    public static string Wander = "Wander";

    

    //Partial Job States
    public static string MoveToDestination = "MoveToDestination";
    public static string DepositItemsToStorage = "DepositItemsToStorage";

    //Stats
    public static string StaminaRecovery = "StanimaRecovery";
    public static string Hungry = "Hungry";
    public static string Thirsty = "Thirsty";


    //JOBS
    public static string Job_Control = "JobControl";
    public static string Job_HarvestGrassControl = "JobHarvestGrass";
    public static string Job_HarvestResource = "JobHarvestResource";
    
    public static string Job_DropItemsToStorage = "JobDropItemsToStorage";
  }
}