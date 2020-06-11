using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.TileSystem {

  public class TileGridManager : MonoBehaviour {

    public  List<TilemapComp> tilemaps = new List<TilemapComp>();
    

    public static  TileGridManager instance;

    public bool showGrid = false;



    public void addTilemap(TilemapComp comp) {
      tilemaps.Add(comp);
    }


    private void OnDrawGizmos() {
      if(!showGrid) return;



      
    }


    private void Awake() {
      if(TileGridManager.instance == null) TileGridManager.instance = this;      
    }

    
    
  } 
}