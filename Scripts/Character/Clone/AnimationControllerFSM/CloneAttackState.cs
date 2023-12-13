using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAttackState : CloneBaseState
{
    public CloneAttackState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.AttackParameterHash);
    }
}

public class CloneComboAttackState : CloneAttackState
{
    public CloneComboAttackState(CloneStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Clone.AnimationData.ComboAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Clone.AnimationData.ComboAttackParameterHash);
    }
}
