using System;
using UnityEngine;
using Pathfinding;

namespace Mlf.Sm.BasicStateMachine.Data {

  [Serializable] 
  public struct BaseData {
    public string name;
  }

  public static  class BaseDataModifiers {

    public static BaseData getBasicNpcData(string name) {
      return new BaseData {
        name=name,
      };
    }

  }
}