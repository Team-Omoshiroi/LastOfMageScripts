using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAimState : CloneBaseState
{
    public CloneAimState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.AimParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.AimParameterHash);
    }
}
