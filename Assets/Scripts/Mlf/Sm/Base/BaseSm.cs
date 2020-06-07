using System;
using System.Collections.Generic;
using Mlf.Sm.State;
using UnityEngine;

namespace Mlf.Sm.Base {



  // State Machine
  public abstract class BaseSm<T> : MonoBehaviour, IBaseSm 
                  where T  : IBaseSm {


    [SerializeField] public Dictionary<string, BaseState<T>> states = 
          new Dictionary<string,  BaseState<T>>();

    [SerializeField] public List<BaseAction<T>> preStateActions = 
          new List<BaseAction<T>>();
    [SerializeField] public List<BaseAction<T>> postStateActions = 
          new List<BaseAction<T>>();

    [SerializeField] public List<BaseAction<T>> preStateFixedActions = 
          new List<BaseAction<T>>();
    [SerializeField] public List<BaseAction<T>> postStateFixedActions = 
          new List<BaseAction<T>>();

    
    [SerializeField] private string _nextState;
    [SerializeField] public  string _currentState;
    public string nextState { get => _nextState; set=> _nextState = value; }

    public BaseState<T> currentState { get; set ; }



    public void SetState(string name, bool useNextStateIfAvailable = false) {
      string newState = name;

      if( useNextStateIfAvailable && 
          this.nextState != null && 
          this.nextState != "") {  

        if(states.ContainsKey(nextState))
          newState = nextState;
      }

      if(newState != currentState.stateName && states.ContainsKey(newState)){
        Debug.Log("Changed State to: "+ newState);
        _currentState = nextState;
        //end prev state
        currentState?.End();
        currentState = states[newState];
        currentState.Start();
      }
      else {
        Debug.LogWarning("Calling unknown state: " + name);
      }
    }


    public virtual void setNextState() {
      if(_nextState != null) {
        SetState(this.nextState);
      }
      Debug.LogError("Next State Not Defined");
    }

    public void InitState(string state)
    {
      currentState = states[state];
      currentState.Start();
    }

    public void AddState(string name, BaseState<T> state) {
      if(!states.ContainsKey(name)){
        Debug.Log("Adding State: " + name);
        this.states.Add(name, state);
      }
      else {
        Debug.LogWarning("State already exists with this name, name: " + name);
      }

    }
    
    protected virtual void Update() {
      foreach (var a in preStateActions)
      {
          if(!a.tick()) break;
      }
       this.currentState.Update();

      foreach (var a in postStateActions)
      {
          if(!a.tick()) break;
      }
    }

    protected virtual void FixedUpdate() {
      foreach (var a in preStateFixedActions)
      {
          if(!a.tick()) break;
      }
      this.currentState.FixedUpdate();
      foreach (var a in postStateFixedActions)
      {
          if(!a.tick()) break;
      }
    }


  }






}