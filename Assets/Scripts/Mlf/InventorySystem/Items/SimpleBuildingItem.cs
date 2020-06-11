using System.Collections;
using System.Collections.Generic;
using Mlf.TileSystem.Tiles;
using UnityEngine;


namespace Mlf.InventorySystem.Items {

[CreateAssetMenu(fileName="New Simple Builidng Item", menuName = "Inventory System/Items/Simple Building")]
  public class SimpleBuildingItem : BaseItem {

    public Sprite whilePlacing;
    public IPathBaseTile tile;
    public void  Awake() {
      type = ItemType.Building;
    }

    public override bool onItemUse() {
      Debug.Log("Building building......");

      return true;
    }
  }


}

