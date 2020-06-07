using System.Collections.Generic;
using UnityEngine;

namespace Mlf.Sm.Base {
  public abstract class BaseAction<T>  where T: IBaseSm
  {
    public T sm;

    protected BaseAction(T sm) {
      this.sm = sm;
    }

    public virtual bool tick() 
    {return true;}
    
  }
}