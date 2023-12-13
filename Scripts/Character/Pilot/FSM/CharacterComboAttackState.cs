using UnityEngine;

public class CharacterComboAttackState : CharacterAttackState
{
    private bool alreadyAppliedForce;
    private bool alreadyApplyCombo;
    private AttackInfo attackInfo;
    private bool _needUpdate;

    public CharacterComboAttackState(CharacterStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        alreadyApplyCombo = false;
        alreadyAppliedForce = false;

        attackInfo = (
            _stateMachine.Character.Equipments.GetEquippedItem(eItemType.Magic) as BaseMagic
        ).AttackData.GetAttackInfo(ComboIndex);
        _stateMachine.Character.Animator.SetInteger(
            _stateMachine.Character.AnimationData.ComboIndexParameterHash,
            ComboIndex
        );
        _stateMachine.Sync?.SendC_ComboAttackPacket(
            ComboIndex,
            _stateMachine.Character.transform.position,
            _stateMachine.Controller.velocity
        );
        _stateMachine.Character.Animator.speed = _stateMachine.Character.Stats.AtkSpeed;
        StartAnimation(_stateMachine.Character.AnimationData.ComboAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Character.Animator.speed = 1.0f;
        StopAnimation(_stateMachine.Character.AnimationData.ComboAttackParameterHash);

        if (!alreadyApplyCombo)
            ComboIndex = 0;
    }

    private void TryComboAttack()
    {
        if (alreadyApplyCombo)
            return;

        if (attackInfo.ComboStateIndex == -1)
            return;

        if (!IsAttacking)
            return;

        alreadyApplyCombo = true;
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce)
            return;
        alreadyAppliedForce = true;

        AttackManager.Instance.RqAttack(
            ComboIndex,
            _stateMachine.Character,
            _stateMachine.Character.transform.position,
            _stateMachine.AttackDirection
        );

        Vector3 newDir = new Vector3()
        {
            x = _stateMachine.AttackDirection.normalized.x,
            z = _stateMachine.AttackDirection.normalized.y,
            y = 0.0f
        };
        if (_stateMachine.Movement == null)
            return;
        if (attackInfo.AttackType < eAttackType.Range)
            _stateMachine.Movement?.AddImpact(
                newDir * _stateMachine.CharacterBaseSpeed * _stateMachine.CharacterSpeedMultiflier,
                0.1f
            );

        // DateTime dateTime = System.DateTime.Now;
        // Debug.Log($"{dateTime.Ticks / 10000 % 1000000000}");

        _stateMachine.Sync?.SendC_MakeAttackAreaPacket(
            ComboIndex,
            _stateMachine.Character.transform.position,
            _stateMachine.AttackDirection
        );
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(
            _stateMachine.Character.Animator,
            _stateMachine.LayerInAnimator,
            "Attack"
        );
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attackInfo.ForceTransitionTime)
                TryApplyForce();
            if (_needUpdate)
                _stateMachine.Sync?.SendC_ComboAttackPacket(
                    ComboIndex,
                    _stateMachine.Character.transform.position,
                    _stateMachine.Controller.velocity
                );
        }
        else
        {
            if (alreadyApplyCombo)
            {
                ComboIndex = attackInfo.ComboStateIndex;
                _stateMachine.ChangeState(eStateType.ComboAttack);
            }
            else
            {
                _stateMachine.ChangeState(eStateType.None);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (_stateMachine.combineStateMachine.GetCurrentStateType(0) == eStateType.Fall || _stateMachine.combineStateMachine.GetCurrentStateType(0) == eStateType.Dodge)
            _stateMachine.ChangeState(eStateType.None);
        else
        {
            if (!_needUpdate)
                _needUpdate = true;
        }
    }

    protected override void AttackEvent(Vector2 direction)
    {
        float normalizedTime = GetNormalizedTime(
            _stateMachine.Character.Animator,
            _stateMachine.LayerInAnimator,
            "Attack"
        );
        if (normalizedTime >= attackInfo.ComboTransitionTime)
            TryComboAttack();
    }

    protected override void AimEvent(Vector2 direction)
    {
        base.AimEvent(direction);
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

        SoundManager.Instance?.Play(attackInfo.AttackSound, type, Random.Range(0.9f, 1.0f),
            Mathf.Clamp(1 / distance, 0.0f, 1.0f));
    }
}
