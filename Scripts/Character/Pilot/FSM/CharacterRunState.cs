using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunState : CharacterWalkState
{
    public CharacterRunState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        _stateMachine.MovementSpeedMultiflier = _stateMachine.CharacterSpeedMultiflier;
        base.Enter();
        StartAnimation(_stateMachine.Character.AnimationData.RunParameterHash);
        _stateMachine.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, _stateMachine.Controller.velocity);
    }

    public override void Exit()
    {
        _stateMachine.MovementSpeedMultiflier = 1.0f;
        base.Exit();
        StopAnimation(_stateMachine.Character.AnimationData.RunParameterHash);
    }
    public override void PhysicsUpdate()
    {
        if (_stateMachine.combineStateMachine.GetCurrentStateType(1) != eStateType.None)
            _stateMachine.ChangeState(eStateType.Walk);
    }
    protected override void RunEvent(bool isRun)
    {
        base.RunEvent(isRun);
        if (!isRun)
        {
            _stateMachine.ChangeState(eStateType.Walk);
        }
    }

    protected override void MoveEvent(Vector2 direction)
    {
        base.MoveEvent(direction);
        if (direction == Vector2.zero)
        {
            _stateMachine.ChangeState(eStateType.Idle);
        }
    }

    protected override void DodgeEvent()
    {
        _stateMachine.ChangeState(eStateType.Dodge);
    }
}
