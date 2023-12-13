using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterFallState : CharacterAirState
{
    private eStateType _nextState;
    private bool _needSend;
    public CharacterFallState(CharacterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _stateMachine.MovementSpeedMultiflier = 0.0f;
        _nextState = _stateMachine.previousStateType;

        StartAnimation(_stateMachine.Character.AnimationData.FallParameterHash);
        _stateMachine.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, _stateMachine.Controller.velocity);
    }

    public override void Exit()
    {
        base.Exit();

        _stateMachine.MovementSpeedMultiflier = 1.0f;

        StopAnimation(_stateMachine.Character.AnimationData.FallParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (CheckGround())
        {
            _stateMachine.ChangeState(_nextState);
            return;
        }
        if (_needSend)
        {
            _stateMachine.Sync?.SendC_MovePacket(
                (int)_stateMachine.currentStateType,
                _stateMachine.Character.transform.position,
                _stateMachine.Controller.velocity);
            _needSend = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!_needSend)
            _needSend = true;
    }

    protected override void RunEvent(bool isRun)
    {
        base.RunEvent(isRun);
    }

    protected override void MoveEvent(Vector2 direction)
    {
        MoveCharacter(direction);
        if (direction == Vector2.zero)
            _nextState = eStateType.Idle;
        else
        {
            if (_isRunning)
                _nextState = eStateType.Run;
            else
                _nextState = eStateType.Walk;
        }
    }
}
