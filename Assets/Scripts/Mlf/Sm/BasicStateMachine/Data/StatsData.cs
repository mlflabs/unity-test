using System;
using UnityEngine;
using Pathfinding;

namespace Mlf.Sm.BasicStateMachine.Data {

  [Serializable] 
  public struct StatsData {
  

    public float intervalTimeSec;
    public float lastUpdateTime;

    //Water
    public float water;
    public float waterMax;
    public float waterUsagePerInterval; 
    

    //Hunber
    public float food;
    public float foodMax;
    public float foodUsagePerInterval;


    //Stanima
    public float stanima;
    public float stanimaMax;
    public float stanimaRecoverPerSec;

  }

  public static  class StatsDataModifiers {

    public static StatsData getBasicData() {
      return new StatsData {
        intervalTimeSec = 5f,
        lastUpdateTime = 0f,
        //Water
        water = 10f,
        waterMax = 20f,
        waterUsagePerInterval = 5f,
        //Food
        food = 20f,
        foodMax = 40f,
        foodUsagePerInterval = 5f,

        //Stanima
        stanima = 50f,
        stanimaMax = 50f,
        stanimaRecoverPerSec = 5f,
      };
    }

    public static StatsData addStanima(float stanima, StatsData data) {
      
      data.stanima += stanima;

      if(data.stanima > data.stanimaMax)
        data.stanima = data.stanimaMax;

      return data;
    }

    public static StatsData updateStats(StatsData data) {
      if(Time.fixedTime < data.lastUpdateTime) return data;
      if(data.lastUpdateTime == 0f) data.lastUpdateTime = Time.fixedTime;

      data.lastUpdateTime = Time.fixedTime + data.intervalTimeSec;
      //update water
      data.water -= data.waterUsagePerInterval;

      //food
      data.food -= data.foodUsagePerInterval;
      return data;
    }

    public static StatsData eat(StatsData data) {
      data.food = data.foodMax;
      return data;
    }

    public static StatsData drink(StatsData data) {
      data.water = data.waterMax;
      return data;
    }


  }
}