using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAirState : CloneBaseState
{
    public CloneAirState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.AirParameterHash);
    }
}

public class CloneFallState : CloneAirState
{
    public CloneFallState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.FallParameterHash);
    }
}
