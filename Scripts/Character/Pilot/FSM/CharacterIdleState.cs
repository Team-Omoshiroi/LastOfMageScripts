using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterIdleState : CharacterGroundState
{
    private bool _needUpdate;
    public CharacterIdleState(CharacterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Character.AnimationData.IdleParameterHash);
        _stateMachine.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, _stateMachine.Controller.velocity);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Character.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (_needUpdate)
        {
            if (_stateMachine.Controller.velocity.sqrMagnitude > 0)
            {
                _stateMachine.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, _stateMachine.Controller.velocity);
            }
            else
            {
                _stateMachine.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, Vector3.zero);
            }
            _needUpdate = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!_needUpdate && _stateMachine.Controller.velocity.sqrMagnitude > 0)
            _needUpdate = true;
    }

    protected override void MoveEvent(Vector2 direction)
    {
        base.MoveEvent(direction);
        if (direction != Vector2.zero)
        {
            if (_isRunning && _stateMachine.combineStateMachine.GetCurrentStateType(1) == eStateType.None)
                _stateMachine.ChangeState(eStateType.Run);
            else
                _stateMachine.ChangeState(eStateType.Walk);
        }
    }

    protected override void RunEvent(bool isRun)
    {
        base.RunEvent(isRun);
    }

    protected override void DodgeEvent()
    {
        _stateMachine.ChangeState(eStateType.Dodge);
    }
}
