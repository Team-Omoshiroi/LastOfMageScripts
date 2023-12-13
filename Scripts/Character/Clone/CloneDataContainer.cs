using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDataContainer : DataContainer
{
    public CloneSync Sync { get; private set; }
    public CloneMovement Movement { get; private set; }

    private CombineCloneStatemachine _stateMachine;

    [Header("테스트용 착용아이템")]
    [SerializeField]
    private BaseItem[] TestEquipItem;

    private void Awake()
    {
        Movement = GetComponent<CloneMovement>();
        Animator = GetComponent<Animator>();
        Sync = GetComponent<CloneSync>();

        AnimationData.Initialize();
    }

    private void Start()
    {
        if (Equipments == null)
            Equipments = new EquipSystem(this);

        foreach (var item in TestEquipItem)
            Equipments.Equip(item);

        _stateMachine = new CombineCloneStatemachine(this);
        SpriteRotator.Register(this);
    }

    public void PlaySound()
    {
        _stateMachine.PlaySound();
    }
}
