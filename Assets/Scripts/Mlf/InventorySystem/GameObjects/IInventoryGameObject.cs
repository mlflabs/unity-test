using Mlf.InventorySystem;
using Mlf.InventorySystem.Items;
using UnityEngine;

namespace Mlf.InventorySystem.GameObjects {
  public interface IInventoryGameObject
  {
     
     InventoryData inventory {get; set;}
     bool canAcceptItem(BaseItem item);
     bool canAcceptType(ItemType type);

      
  }
}