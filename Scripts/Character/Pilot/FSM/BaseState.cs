using Unity.VisualScripting;
using UnityEngine;

public class BaseState : IState
{
    protected CharacterStateMachine _stateMachine;
    static protected bool _isRunning;
    public BaseState(CharacterStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public virtual void PlaySound()
    {

    }
    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        
    }

    // Input
    protected virtual void AddInputActionsCallbacks()
    {
        BaseInput input = _stateMachine.InputActions;
        input.OnMoveEvent += MoveEvent;
        input.OnRunEvent += RunEvent;
        input.OnAttackEvent += AttackEvent;
        input.OnAimEvent += AimEvent;
        input.OnDodgeEvent += DodgeEvent;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        BaseInput input = _stateMachine.InputActions;
        input.OnMoveEvent -= MoveEvent;
        input.OnRunEvent -= RunEvent;
        input.OnAttackEvent -= AttackEvent;
        input.OnAimEvent -= AimEvent;
        input.OnDodgeEvent -= DodgeEvent;
    }

    protected virtual void RunEvent(bool isRun)
    {
        // 달리기
        _isRunning = isRun;
    }

    protected virtual void MoveEvent(Vector2 direction)
    {
        // 이동
        // MoveCharacter(direction);
    }

    protected virtual void AttackEvent(Vector2 direction)
    {
        // TODO
        // 공격
    }

    protected virtual void AimEvent(Vector2 direction)
    {
        // TODO
        // 조준
        direction.Normalize();
        _stateMachine.AttackDirection = new Vector3() { x = direction.x, y = 0, z = direction.y };
    }

    protected virtual void DodgeEvent()
    {
        // TODO
        // HealthSystem에 무적 적용
    }

    protected void StartAnimation(int animationHash)
    {
        _stateMachine.Character.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        _stateMachine.Character.Animator.SetBool(animationHash, false);
    }

    protected float GetNormalizedTime(Animator animator, int layer, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(layer);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(layer);
        if (animator.IsInTransition(layer) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(layer) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    protected Vector3 MoveCharacter(Vector2 direction)
    {
        if (_stateMachine.Movement == null)
            return Vector3.zero;
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        direction.Normalize();

        _stateMachine.Movement.ControlMove(forward * direction.y + right * direction.x);
        return forward * direction.y + right * direction.x;
    }

    protected bool CheckGround()
    {
        if (_stateMachine.Controller.isGrounded) return true;

        Ray downRay = new Ray(_stateMachine.Character.transform.position, Vector3.down);

        if (Physics.Raycast(downRay, 1.5f, _stateMachine.Character.AnimationData.GroundLayer))
        {
            return true;
        }
        return false;
    }

    public virtual void SetAnimation(Animator animator, int layer, float normalizeTime)
    {
        var state = animator.GetCurrentAnimatorStateInfo(layer);
        animator.Play(state.fullPathHash, layer, normalizeTime);
    }
}

public class CloneBaseState : IState
{
    protected CloneStateMachine _stateMachine;

    public CloneBaseState(CloneStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        //AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        //RemoveInputActionsCallbacks();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {

    }

    // Input
    //protected virtual void AddInputActionsCallbacks()
    //{
    //    CloneSync sync = _stateMachine.Sync;

    //    //sync.OnMoveEvent += MoveEvent;
    //    //sync.OnRunEvent += RunEvent;
    //    //sync.OnAttackEvent += AttackEvent;
    //    //sync.OnAimEvent += AimEvent;
    //    //sync.OnDodgeEvent += DodgeEvent;
    //}

    //protected virtual void RemoveInputActionsCallbacks()
    //{
    //    CloneSync sync = _stateMachine.Sync;

    //    //sync.OnMoveEvent -= MoveEvent;
    //    //sync.OnRunEvent -= RunEvent;
    //    //sync.OnAttackEvent -= AttackEvent;
    //    //sync.OnAimEvent -= AimEvent;
    //    //sync.OnDodgeEvent -= DodgeEvent;
    //}

    //protected virtual void RunEvent(bool isRun)
    //{
    //    // 달리기
    //}

    //protected virtual void MoveEvent(Vector2 direction)
    //{
    //    // 이동
    //    // MoveCharacter(direction);
    //}

    //protected virtual void AttackEvent(Vector2 direction)
    //{
    //    // TODO
    //    // 공격
    //}

    //protected virtual void AimEvent(Vector2 direction)
    //{
    //    // TODO
    //    // 조준
    //}

    //protected virtual void DodgeEvent()
    //{
    //    // TODO
    //    // HealthSystem에 무적 적용
    //}

    protected void StartAnimation(int animationHash)
    {
        _stateMachine.Clone.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        _stateMachine.Clone.Animator.SetBool(animationHash, false);
    }

    protected float GetNormalizedTime(Animator animator, int layer, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(layer);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(layer);
        if (animator.IsInTransition(layer) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(layer) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    protected bool CheckGround()
    {
        Ray downRay = new Ray(_stateMachine.Clone.transform.position, Vector3.down);

        if (Physics.Raycast(downRay, 1.5f, _stateMachine.Clone.AnimationData.GroundLayer))
        {
            return true;
        }
        return false;
    }

    public virtual void SetAnimation(Animator animator, int layer, float normalizeTime)
    {
        var state = animator.GetCurrentAnimatorStateInfo(layer);
        animator.Play(state.fullPathHash, layer, normalizeTime);
    }

    public virtual void PlaySound()
    {
        
    }
}