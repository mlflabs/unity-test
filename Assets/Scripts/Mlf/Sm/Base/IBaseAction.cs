using System.Collections.Generic;
using UnityEngine;

namespace Mlf.Sm.Base {
  public interface IBaseAction<T> where T:  IBaseSm {

    //if returns false, stop
    bool tick();
  }

}