using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDodgeState : BaseState
{
    float passedTime;
    float duration;
    private bool alreadyAppliedForce;

    private bool _needUpdate;
    private eStateType _nextState;

    public CharacterDodgeState(CharacterStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        duration = _stateMachine.Character.Stats.DodgeTime;
        passedTime = 0.1f;
        alreadyAppliedForce = false;
        _nextState = _stateMachine.previousStateType;
        StartAnimation(_stateMachine.Character.AnimationData.DodgeParameterHash);
        _stateMachine.Sync?.SendC_DodgePacket(
            _stateMachine.Character.transform.position,
            _stateMachine.Controller.velocity
        );
        // 무적 적용
        _stateMachine.Character.Health.IsDodge = true;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Character.AnimationData.DodgeParameterHash);
    }

    public override void Update()
    {
        float normalizedTime = GetNormalizedTime(
            _stateMachine.Character.Animator,
            _stateMachine.LayerInAnimator,
            "Dodge"
        );
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= passedTime)
            {
                TryApplyForce();
            }
            if (_needUpdate)
                _stateMachine.Sync?.SendC_DodgePacket(
                    _stateMachine.Character.transform.position,
                    _stateMachine.Controller.velocity
                );
            if (normalizedTime > duration)
            {
                // 무적 풀기
                _stateMachine.Character.Health.IsDodge = false;
            }
        }
        else
        {
            _stateMachine.ChangeState(_nextState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!_needUpdate)
            _needUpdate = true;
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce)
            return;
        alreadyAppliedForce = true;
        if (_stateMachine.Movement == null)
            return;
        if (_stateMachine.Movement.ControlDirection == Vector3.zero)
        {
            _stateMachine.Movement.AddImpact(
                _stateMachine.AttackDirection.normalized
                    * _stateMachine.CharacterBaseSpeed
                    * _stateMachine.CharacterSpeedMultiflier
                    * 5,
                0.5f
            );
        }
        else
            _stateMachine.Movement.AddImpact(
                _stateMachine.Movement.ControlDirection
                    * _stateMachine.CharacterBaseSpeed
                    * _stateMachine.CharacterSpeedMultiflier
                    * 5,
                0.5f
            );
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

    protected override void RunEvent(bool isRun)
    {
        base.RunEvent(isRun);
        if (_isRunning)
            _nextState = eStateType.Run;
        else if (_nextState != eStateType.Idle)
            _nextState = eStateType.Walk;
    }

    public override void PlaySound()
    {
        base.PlaySound();
        var distance = (DataContainer.AudioListener.position - _stateMachine.Character.transform.position).magnitude;
        eSoundType type = eSoundType.OtherEffect;
        if (distance < 1.0f)
        {
            distance = 1.0f;
            type = eSoundType.PlayerEffect;
        }
        else if (distance > 20.0f)
            return;

        SoundManager.Instance.Play(_stateMachine.Character.AnimationData.DodgeSound, type, Random.Range(0.9f, 1.0f),
            Mathf.Clamp(1 / distance, 0.0f, 1.0f));
    }
}
