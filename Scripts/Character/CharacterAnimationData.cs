using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string dodgeParameterName = "Dodge";

    [SerializeField] private string airParameterName = "@Air";
    [SerializeField] private string fallParameterName = "Fall";

    [SerializeField] private string aimParameterName = "@Aim";
    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string comboAttackParameterName = "ComboAttack";
    [SerializeField] private string comboIndexParameterName = "Combo";
    public LayerMask GroundLayer;
    public LayerMask StoneLayer;
    [field: SerializeField] public List<AudioClip> StepOnStone { get; private set; }
    public LayerMask GrassLayer;
    [field: SerializeField] public List<AudioClip> StepOnGrass { get; private set; }
    [field: SerializeField] public AudioClip DodgeSound{ get; private set; }


    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int DodgeParameterHash { get; private set; }

    public int AirParameterHash { get; private set; }
    public int FallParameterHash { get; private set; }

    public int AimParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int ComboAttackParameterHash { get; private set; }
    public int ComboIndexParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        DodgeParameterHash = Animator.StringToHash(dodgeParameterName);
        AimParameterHash = Animator.StringToHash(aimParameterName);

        AirParameterHash = Animator.StringToHash(airParameterName);
        FallParameterHash = Animator.StringToHash(fallParameterName);

        AttackParameterHash = Animator.StringToHash(attackParameterName);
        ComboAttackParameterHash = Animator.StringToHash(comboAttackParameterName);
        ComboIndexParameterHash = Animator.StringToHash(comboIndexParameterName);
    }
}
