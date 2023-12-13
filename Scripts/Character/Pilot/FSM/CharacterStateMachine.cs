using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterStateMachine : StateMachine
{
    // Character Info
    public DataContainer Character { get; }
    public BaseInput InputActions { get; }
    public CharacterMovement Movement { get; }
    public PilotSync Sync { get; }
    public CharacterController Controller { get; }
    public float CharacterSpeedMultiflier
    {
        get => Character.Stats.RunMultiplier;
    }
    public float CharacterBaseSpeed
    {
        get => Character.Stats.MoveSpeed;
    }
    public CombineStateMachine combineStateMachine { get; }

    // State Info
    public float MovementSpeedMultiflier
    {
        set
        {
            if (Movement != null)
                Movement.SpeedMultiflier = value;
        }
    }
    public int LayerInAnimator { get; }
    public Vector3 AttackDirection { get; set; }

    public CharacterStateMachine(
        CombineStateMachine combine,
        CharacterDataContainer character,
        int layerInAnimator
    )
    {
        Character = character;
        InputActions = character.InputActions;
        LayerInAnimator = layerInAnimator;
        combineStateMachine = combine;
        Controller = character.Controller;

        Sync = character.Sync;
        if (character.Movement != null)
            Movement = character.Movement;
    }

    public CharacterStateMachine(CombineStateMachine combine,
        NPCDataContainer container,
        int layerInAnimator)
    {
        Character = container;
        InputActions = container.InputActions;
        LayerInAnimator = layerInAnimator;
        combineStateMachine = combine;
        Controller = container.Controller;
    }
}
