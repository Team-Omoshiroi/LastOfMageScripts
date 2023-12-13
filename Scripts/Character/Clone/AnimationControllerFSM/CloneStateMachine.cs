using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneStateMachine : StateMachine
{
    // Character Info
    public CloneDataContainer Clone { get; }
    public CloneSync Sync { get; }
    public CloneMovement Movement { get; }

    // Animation Info
    public int LayerInAnimator { get; }

    public CloneStateMachine(CloneDataContainer clone, int layerInAnimator)
    {
        Clone = clone;
        Sync = Clone.Sync;
        Movement = Clone.Movement;
        LayerInAnimator = layerInAnimator;
    }
}

public class CombineCloneStatemachine
{
    CloneStateMachine[] _stateMachine;

    public CombineCloneStatemachine(CloneDataContainer clone)
    {
        _stateMachine = new CloneStateMachine[2];

        _stateMachine[0] = new CloneStateMachine(clone, 0);
        _stateMachine[0].AddState(eStateType.Idle, new CloneIdleState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Walk, new CloneWalkState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Run, new CloneRunState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Dodge, new CloneDodgeState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Fall, new CloneFallState(_stateMachine[0]));
        _stateMachine[0].ChangeState(eStateType.Idle);

        _stateMachine[1] = new CloneStateMachine(clone, 1);
        _stateMachine[1].AddState(
            eStateType.ComboAttack,
            new CloneComboAttackState(_stateMachine[1])
        );
        _stateMachine[1].AddState(eStateType.Aim, new CloneAimState(_stateMachine[1]));
        _stateMachine[1].AddState(eStateType.None, new CloneNoneState(_stateMachine[1]));
        _stateMachine[1].ChangeState(eStateType.None);

        if (clone.Sync != null)
            SyncInit(clone.Sync);
    }

    public void SyncInit(CloneSync sync)
    {
        sync.OnMoveEvent += CloneMoveEvent;
        sync.OnAimEvent += CloneAimEvent;
        sync.OnComboAttackEvent += CloneComboAttackEvent;
        sync.OnMakeAttackAreaEvent += CloneMakeAttackAreaEvent;
        sync.OnDodgeEvent += CloneDodgeEvent;
    }

    public void Update()
    {
        foreach (var stateMachine in _stateMachine)
            stateMachine.Update();
    }

    public void PhysicsUpdate()
    {
        foreach (var stateMachine in _stateMachine)
            stateMachine.PhysicsUpdate();
    }

    public void ChangeState(eStateType state)
    {
        foreach (var stateMachine in _stateMachine)
        {
            if (stateMachine.States.ContainsKey(state))
            {
                if (stateMachine.currentStateType == state && state != eStateType.ComboAttack)
                    break;
                stateMachine.ChangeState(state);
                break;
            }
        }
    }

    public void SetAnimation(eStateType state, float normalizeTime)
    {
        if (
            _stateMachine[0].currentStateType != state && _stateMachine[1].currentStateType != state
        )
            ChangeState(state);
        foreach (var stateMachine in _stateMachine)
        {
            if (stateMachine.currentStateType == state)
            {
                stateMachine.CurrentState.SetAnimation(
                    stateMachine.Clone.Animator,
                    stateMachine.LayerInAnimator,
                    normalizeTime
                );
                break;
            }
        }
    }

    //public void CloneAttackEvent(int comboIndex, Vector3 position, Vector3 direction)
    //{
    //    // 공격 생성
    //    AttackManager.Instance.RqAttack(comboIndex, _stateMachine[0].Clone, position, direction);
    //}

    public void CloneMakeAttackAreaEvent(int comboIndex, Vector3 position, Vector3 direction)
    {
        // 공격 생성
        AttackManager.Instance.RqAttack(comboIndex, _stateMachine[0].Clone, position, direction);
    }

    public void CloneMoveEvent(int state, Vector3 posInfo, Vector3 velInfo)
    {
        // 걷기, 달리기, 낙하
        eStateType type = (eStateType)state;
        if (_stateMachine[0].currentStateType != type && _stateMachine[1].currentStateType != type)
            ChangeState(type);
    }

    //public void CloneBattleEvent(int state, float animTime, Vector3 posInfo, Vector3 velInfo)
    //{
    //    // 회피, 공격 애니메이션 시작
    //    eStateType type = (eStateType)state;
    //    if (type == eStateType.Dodge || type == eStateType.ComboAttack)
    //    {
    //        if (_stateMachine[0].currentStateType != type && _stateMachine[1].currentStateType != type)
    //            ChangeState(type);
    //        //SetAnimation(type, animTime);
    //    }
    //}

    public void CloneDodgeEvent(Vector3 posInfo, Vector3 velInfo)
    {
        // 회피 애니메이션 시작
        if (_stateMachine[0].currentStateType != eStateType.Dodge)
            ChangeState(eStateType.Dodge);
    }

    public void CloneComboAttackEvent(int comboindex, Vector3 posInfo, Vector3 dirInfo)
    {
        // 공격 애니메이션 시작
        if (_stateMachine[1].currentStateType != eStateType.ComboAttack)
            ChangeState(eStateType.ComboAttack);
    }

    public void CloneAimEvent(int state, Vector3 velInfo)
    {
        // 조준 애니메이션 시작
        if (_stateMachine[1].currentStateType != eStateType.Aim)
            ChangeState(eStateType.Aim);
    }

    public void PlaySound()
    {
        foreach (var stateMachine in _stateMachine)
            stateMachine.CurrentState.PlaySound();
    }
}
