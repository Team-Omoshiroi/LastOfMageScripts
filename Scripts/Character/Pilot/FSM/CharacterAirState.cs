using Unity.VisualScripting;
using UnityEngine;

public class CharacterAirState : BaseState
{
    public CharacterAirState(CharacterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Character.AnimationData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Character.AnimationData.AirParameterHash);
    }
}
