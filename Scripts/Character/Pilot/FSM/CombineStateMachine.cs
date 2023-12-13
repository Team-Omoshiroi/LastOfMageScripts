using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CombineStateMachine
{
    public CharacterStateMachine[] _stateMachine { get; private set; }

    public CombineStateMachine(CharacterDataContainer character)
    {
        _stateMachine = new CharacterStateMachine[2];

        _stateMachine[0] = new CharacterStateMachine(this, character, 0);
        _stateMachine[0].AddState(eStateType.Idle, new CharacterIdleState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Walk, new CharacterWalkState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Run, new CharacterRunState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Fall, new CharacterFallState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Dodge, new CharacterDodgeState(_stateMachine[0]));
        _stateMachine[0].ChangeState(eStateType.Idle);

        _stateMachine[1] = new CharacterStateMachine(this, character, 1);
        _stateMachine[1].AddState(eStateType.ComboAttack, new CharacterComboAttackState(_stateMachine[1]));
        _stateMachine[1].AddState(eStateType.Aim, new CharacterAimState(_stateMachine[1]));
        _stateMachine[1].AddState(eStateType.None, new CharacterNoneState(_stateMachine[1]));
        _stateMachine[1].ChangeState(eStateType.None);
    }

    public CombineStateMachine(NPCDataContainer container)
    {
        _stateMachine = new CharacterStateMachine[2];

        _stateMachine[0] = new CharacterStateMachine(this, container, 0);
        _stateMachine[0].AddState(eStateType.Idle, new CharacterIdleState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Walk, new CharacterWalkState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Run, new CharacterRunState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Fall, new CharacterFallState(_stateMachine[0]));
        _stateMachine[0].AddState(eStateType.Dodge, new CharacterDodgeState(_stateMachine[0]));
        _stateMachine[0].ChangeState(eStateType.Idle);

        _stateMachine[1] = new CharacterStateMachine(this, container, 1);
        _stateMachine[1].AddState(eStateType.ComboAttack, new CharacterComboAttackState(_stateMachine[1]));
        _stateMachine[1].AddState(eStateType.Aim, new CharacterAimState(_stateMachine[1]));
        _stateMachine[1].AddState(eStateType.None, new CharacterNoneState(_stateMachine[1]));
        _stateMachine[1].ChangeState(eStateType.None);
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

    public eStateType GetCurrentStateType(int layer)
    {
        return _stateMachine[layer].currentStateType;
    }

    public void PlaySound()
    {
        foreach (var stateMachine in _stateMachine)
            stateMachine.PlaySound();
    }
}
