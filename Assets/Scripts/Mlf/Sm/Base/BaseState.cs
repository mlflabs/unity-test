using System;
using UnityEngine;

namespace Mlf.Sm.Base {


  [Serializable]  public abstract class BaseState<T> where T: IBaseSm               
  {
    public T sm { get; set; }

    public string stateName;
    public string baseStateName;
    
    protected BaseState(T sm)
    {
      this.sm = sm;
    }

    public virtual void Update()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void End() {
    }

    public virtual void Start() {
    }
  }
}