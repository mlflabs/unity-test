using System.Collections.Generic;
using Mlf.InventorySystem.Items;
using Mlf.Gm;
using UnityEngine;


namespace Mlf.InventorySystem.GameObjects {


  public abstract class BaseInventoryComp : MonoBehaviour, IInventoryGameObject
  {

    public List<ItemType> canHoldItemTypes = new List<ItemType>();
    public bool canHoldAllTypes = false;
    [SerializeField] private InventoryData _inventory;
    public InventoryData inventory {get => _inventory; set=>_inventory = value;}

    public virtual bool acceptItem(BaseItem item) {

        if(canAcceptItem(item) == false){
          Debug.Log("Can't accept item");
          return false;
        } 

        if(this.inventory.MaxInventoryReached()){
          return false;
        }
        else {
          this.inventory.AddItem(item);
          return true;
        }
      }


      public virtual bool canAcceptItem(BaseItem item){
        Debug.Log("ITEM TYPE::::: " + item);
        return canAcceptType(item.type);
      }
      public bool canAcceptType(ItemType type){
        if(canHoldAllTypes) return true;

        for(int i = 0; i < canHoldItemTypes.Count; i++) {
          if(canHoldItemTypes[i] == type)
            return true;
        }

        return false;
      }

      public virtual bool isFull() {
        return inventory.MaxInventoryReached();
      }


      public virtual bool isEmpty(){
        return inventory.items.Count == 0;
      }




  }
}
