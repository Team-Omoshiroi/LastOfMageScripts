using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneGroundState : CloneBaseState
{
    public CloneGroundState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.GroundParameterHash);
    }
}

public class CloneIdleState : CloneGroundState
{
    public CloneIdleState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.IdleParameterHash);
    }
}

public class CloneWalkState : CloneGroundState
{
    public CloneWalkState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.WalkParameterHash);
    }
}

public class CloneRunState : CloneGroundState
{
    public CloneRunState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.RunParameterHash);
    }
}
