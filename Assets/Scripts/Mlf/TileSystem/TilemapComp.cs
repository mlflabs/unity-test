
using System.Collections.Generic;
using Mlf.InventorySystem;
using Mlf.InventorySystem.GameObjects;
using Mlf.Sm.BasicStateMachine;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.TileSystem
{
  public class TilemapComp : MonoBehaviour {

    Tilemap tilemap;

    void Start() {
      Tilemap tilemap = GetComponent<Tilemap>();
    }
    
  } 
}



