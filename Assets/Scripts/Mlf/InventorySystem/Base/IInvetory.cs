using Mlf.InventorySystem.Items;
using UnityEngine;

namespace Mlf.InventorySystem.Base {
  public interface IInventory
  {
      void AddItem(BaseItem slot);
      BaseItem removeRandomItem();

      void removeOneItemAmountByIndex(int index);

      bool hasMoreItems();
      
  }
}