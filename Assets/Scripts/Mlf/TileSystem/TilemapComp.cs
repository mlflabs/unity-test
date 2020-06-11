
using System.Collections.Generic;
using Mlf.InventorySystem;
using Mlf.InventorySystem.GameObjects;
using Mlf.Sm.BasicStateMachine;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Mlf.TileSystem
{
  public class TilemapComp : MonoBehaviour {

    public Tilemap tilemap;
    public TilemapType type;

    void Start() {
      Tilemap tilemap = GetComponent<Tilemap>();


    }
    
  } 

  public enum TilemapType {


  }
}



