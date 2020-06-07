using UnityEngine;

namespace Mlf.Sm.Base {
  public interface IBaseSm
  {
      void SetState(string state, bool useNextStateIfAvailable);

      void InitState(string state);

      //IBaseState nextState {get; set;} 
      //IBaseState currentState {get; set;}
      
  }
}