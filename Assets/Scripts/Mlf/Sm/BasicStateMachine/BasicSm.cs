
using Mlf.InventorySystem;
using Mlf.Sm.Base;
using Mlf.Sm.BasicStateMachine.Data;
using Mlf.Sm.BasicStateMachine.States;
using Pathfinding;
using UnityEngine;

namespace Mlf.Sm.BasicStateMachine {

  public abstract class BasicSm : BaseSm<BasicSm>, IBaseSm, IBasicSm
  {

    public InventoryData inventory;

    [SerializeField] private BaseData _baseData;
    public BaseData baseData { get => _baseData; set => _baseData = value; }

    [SerializeField] private StatsData _statsData = StatsDataModifiers.getBasicData();
    public StatsData statsData { get => _statsData; set => _statsData = value; }



    [SerializeField] private MotionData _motionData;
    public MotionData motionData { get => _motionData; set => _motionData = value; }

    public Animator animator { get; set; }
    public Rigidbody2D rb { get; set; }

    public Seeker seeker {get; set;}

    [SerializeField] private JobData _jobData;
    public JobData jobData {get => _jobData; set=> _jobData = value;}

    protected virtual void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      animator = GetComponent<Animator>();
      if(animator == null) {
        animator = GetComponentInChildren<Animator>();
      }
      Debug.Log("ANIMATOR::::: " + animator);
      seeker = GetComponent<Seeker>();
    }



  }
}