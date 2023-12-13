using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDodgeState : CloneBaseState
{
    public CloneDodgeState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.DodgeParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.DodgeParameterHash);
    }
}
