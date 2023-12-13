using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNoneState : BaseState
{
    public CharacterNoneState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.Sync?.SendC_AimPacket((int)eStateType.None, Vector3.zero);
    }
    public override void Exit() { base.Exit(); }

    protected override void AimEvent(Vector2 direction)
    {
        if (_stateMachine.combineStateMachine.GetCurrentStateType(0) != eStateType.Fall && _stateMachine.combineStateMachine.GetCurrentStateType(0) != eStateType.Dodge)
        {
            if (direction.magnitude >= 100f)
                _stateMachine.ChangeState(eStateType.Aim);
        }
        base.AimEvent(direction);
    }

    protected override void AttackEvent(Vector2 direction)
    {
        if (_stateMachine.combineStateMachine.GetCurrentStateType(0) != eStateType.Fall && _stateMachine.combineStateMachine.GetCurrentStateType(0) != eStateType.Dodge)
            _stateMachine.ChangeState(eStateType.ComboAttack);
    }
}
