using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.TileSystem {

  public class TileGridManager : MonoBehaviour {

    [SerializeField]private GameObject _selectedObject;
    

    public static  TileGridManager instance;


    private void Awake() {
      if(TileGridManager.instance == null) TileGridManager.instance = this;
      
    }

    
    
  } 
}