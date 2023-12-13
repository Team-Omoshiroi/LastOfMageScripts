using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimState : BaseState
{
    public CharacterAimState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        _stateMachine.MovementSpeedMultiflier = 0.5f;

        StartAnimation(_stateMachine.Character.AnimationData.AimParameterHash);
        _stateMachine.Sync?.SendC_AimPacket((int)eStateType.Aim, _stateMachine.AttackDirection);
    }

    public override void Exit()
    {
        base.Exit();

        _stateMachine.MovementSpeedMultiflier = 1.0f;

        StopAnimation(_stateMachine.Character.AnimationData.AimParameterHash);
    }

    public override void PhysicsUpdate()
    {
        if (_stateMachine.combineStateMachine.GetCurrentStateType(0) == eStateType.Fall || _stateMachine.combineStateMachine.GetCurrentStateType(0) == eStateType.Dodge)
            _stateMachine.ChangeState(eStateType.None);
    }

    protected override void AimEvent(Vector2 direction)
    {
        if (direction.magnitude < 100f)
            _stateMachine.ChangeState(eStateType.None);
        else
        {
            _stateMachine.Sync?.SendC_AimPacket((int)eStateType.Aim, _stateMachine.AttackDirection);
            base.AimEvent(direction);
        }
    }

    protected override void AttackEvent(Vector2 direction)
    {
        _stateMachine.ChangeState(eStateType.ComboAttack);
    }
}
