using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using Mlf.InventorySystem.Items;

namespace Mlf.InventorySystem.Base {

  
  public abstract class BaseInventory :  IInventory {

    public event System.Action onInventoryUpdate;
    public int maxItemSlots = 4;

    public int maxItemCapacity = 16;
    public int visibleItemSlots = 4;
    public int currentItemUsedCapacity = 0;
    public List<InventorySlot> items = new List<InventorySlot>();
    
    public void AddItem(BaseItem item) {
      bool hasItem = false;


      for(int i = 0; i < items.Count; i ++) {
        if(items[i].item == item){
          hasItem = true;
          items[i].AddAmount(1);
          currentItemUsedCapacity++;
          break;
        }
      }

      if(hasItem) return;


      if(items.Count < maxItemSlots){
        items.Add(new InventorySlot(item, 1));
        currentItemUsedCapacity++;
      }
      else {
        Debug.LogError("Inventory overflow::: "+ items.Count);
      }

      onInventoryUpdate?.Invoke();
    }

    public BaseItem removeRandomItem() {
      Debug.Log(items.Count);
      int randomIndex = Random.Range (0, items.Count-1);
      Debug.Log("Count: " + items.Count + "Random index: " + randomIndex);
      BaseItem item = items[randomIndex].item;
      removeOneItemAmountByIndex(randomIndex);
      return item;
    }

    public void removeOneItemAmountByIndex(int index) {
      items[index].amount--;
      currentItemUsedCapacity--;
      if(items[index].amount <= 0){
        items.RemoveAt(index);
      }
      onInventoryUpdate?.Invoke();
    } 

    public bool MaxInventoryReached() {
      if(currentItemUsedCapacity >= maxItemCapacity) return true;

      return false;
    }

    public bool hasMoreItems()
    {
      return items.Count > 0;
    }
  }
}