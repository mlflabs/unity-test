
using System.Collections.Generic;
using Mlf.InventorySystem.GameObjects;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.Gm.Spawners
{
  public class NonRayCastOpenAreaSpawner : BaseSpawner {

    public float seperationDistanceRequirementSQR = 0.5f;


    public override void spawn() {
      if(spanwedList.Count >= maxObjectCount) return;

      

      //test if there is anything close, if there is go again
      bool openArea = true;

      while(true) {
        openArea = true;

        Vector2 point = (Random.insideUnitCircle * 
                      spawnRadiusDistance) + (Vector2) transform.position;


        float distanceFromObject = 0f;
        for(int i = 0; i < spanwedList.Count; i ++) {
          distanceFromObject = (point - (Vector2) spanwedList[i].transform.position).sqrMagnitude;
          if(distanceFromObject < seperationDistanceRequirementSQR){
            openArea = false;
            break;
          }
        }

        if(openArea){
          GameObject go = Instantiate(prefab, point, Quaternion.identity);

          spanwedList.Add(go.GetComponent<HarvestItemComp>());
          HarvestItemComp item = go.GetComponent<HarvestItemComp>();
          //Debug.Log(item);
          if(item == null)
            return;
          //add to gameresourcemanager, register events
          registerItem(item);
          //leave the wile loop we are done
          break;
        }
      }      
    }

    

    
    
  } 
}



