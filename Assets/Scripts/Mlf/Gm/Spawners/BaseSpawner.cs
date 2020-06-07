
using System.Collections.Generic;
using Mlf.InventorySystem;
using Mlf.InventorySystem.GameObjects;
using Mlf.Sm.BasicStateMachine;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.Gm.Spawners
{
  public class BaseSpawner : MonoBehaviour {

    public GameObject prefab;
    public string type;
    public float spawnRadiusDistance = 5f;
    public int maxObjectCount = 10;
    public List<HarvestItemComp> spanwedList = new List<HarvestItemComp>();
 

    public virtual void spawn() {
      if(spanwedList.Count >= maxObjectCount) return;

      Vector2 point = (Random.insideUnitCircle * 
                      spawnRadiusDistance) + (Vector2) transform.position;

      GameObject go = Instantiate(prefab, point, Quaternion.identity);

      HarvestItemComp item = go.GetComponent<HarvestItemComp>();
      //Debug.Log(item);
      if(item == null)
        return;

      spanwedList.Add(item);

      registerItem(item);
    }

    protected void onItemDestroyed(HarvestItemComp item) {
      spanwedList.Remove(item);
    }
 

    public virtual void registerItem(HarvestItemComp item) {
      GameResourceManager.instance.addHarvestItem(item);
      spanwedList[spanwedList.Count-1].onItemDestroyed += onItemDestroyed;
    }
  } 
}



