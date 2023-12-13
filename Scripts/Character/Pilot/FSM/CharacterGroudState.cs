using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterGroundState : BaseState
{
    public CharacterGroundState(CharacterStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Character.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Character.AnimationData.GroundParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // TODO
        // 이거 잘 작동하는지 확인이 필요함
        if (
            !CheckGround()
            && Mathf.Abs(_stateMachine.Controller.velocity.y)
                > Mathf.Abs(Physics.gravity.y * Time.fixedDeltaTime)
        )
        {
            _stateMachine.ChangeState(eStateType.Fall);
            return;
        }
    }

    protected override void MoveEvent(Vector2 direction)
    {
        var direc = MoveCharacter(direction);
        //if (direction == Vector2.zero)
        //    _stateMachine.Character.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, Vector3.zero);
        //else
        //    _stateMachine.Character.Sync?.SendC_MovePacket((int)_stateMachine.currentStateType, _stateMachine.Character.transform.position, direc);
    }

    public override void PlaySound()
    {
        base.PlaySound();
        Ray downRay = new Ray(_stateMachine.Character.transform.position, Vector3.down);

        var hit = Physics.RaycastAll(downRay, 2f, _stateMachine.Character.AnimationData.GroundLayer);
        if (hit.Length > 0)
        {
            var distance = (DataContainer.AudioListener.position - _stateMachine.Character.transform.position).magnitude;
            eSoundType type = eSoundType.OtherEffect;
            if (distance < 1.0f)
            {
                distance = 1.0f;
                type = eSoundType.PlayerEffect;
            }
            else if (distance > 20.0f)
                return;

            if ((1 << hit[0].collider.gameObject.layer) == ((1 << hit[0].collider.gameObject.layer) & _stateMachine.Character.AnimationData.StoneLayer))
            {

                SoundManager.Instance.Play(_stateMachine.Character.AnimationData.StepOnStone[Random.Range(0, _stateMachine.Character.AnimationData.StepOnStone.Count-1)], type,
                    1.0f, Mathf.Clamp(1 / distance, 0.0f, 1.0f));
            }
            else if ((1 << hit[0].collider.gameObject.layer) == ((1 << hit[0].collider.gameObject.layer) & _stateMachine.Character.AnimationData.GrassLayer))
            {
                SoundManager.Instance.Play(_stateMachine.Character.AnimationData.StepOnGrass[Random.Range(0, _stateMachine.Character.AnimationData.StepOnGrass.Count-1)], type,
                    1.0f, Mathf.Clamp(1 / distance, 0.0f, 1.0f));
            }
        }
    }
}
