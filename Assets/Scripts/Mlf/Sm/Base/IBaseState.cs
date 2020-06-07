using System.Collections.Generic;
using UnityEngine;

namespace Mlf.Sm.Base {
  public interface IBaseState
  {
    IBaseSm sm { get; set; }

    string stateName {get; set;}

    // stateName can have several options pointing to same basse state
    // for example, wander, walk, chase, are all move state or
    // idle, wait, idleForRandomTime are idle states
    string baseName {get; set;} 


    void Start(IBaseSm sm);
    void End(IBaseSm sm);
    void Update(IBaseSm sm);
    void FixedUpdate(IBaseSm sm);


  }
}