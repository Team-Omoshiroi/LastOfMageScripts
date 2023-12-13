using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterDataContainer : DataContainer
{
    public CharacterController Controller { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public PlayerInput InputActions { get; private set; }
    public PilotSync Sync { get; private set; }

    private CombineStateMachine stateMachine;

    [Header("테스트용 착용아이템")]
    [SerializeField]
    private BaseItem[] TestEquipItem;
    [Header("테스트용 스탯")]
    [SerializeField]
    private bool isApply;
    [SerializeField]
    private CharacterBaseStats TestStats;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Movement = GetComponent<CharacterMovement>();
        InputActions = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();
        Sync = GetComponent<PilotSync>();
        Health = GetComponent<HealthSystem>();

        AnimationData.Initialize();

        AudioListener = transform;
    }

    private void Start()
    {
        CameraMovement.Instance?.AttachToPlayer(transform);

        // 플래이어 HP 화면 표시 연결
        Stats.OnHpChange += () => { UIController.Instance.HandlerHp(Stats.MaxHp, Stats.Hp); };

        if (Equipments == null)
            Equipments = new EquipSystem(this);

        foreach (var item in TestEquipItem)
            Equipments.Equip(item);

        stateMachine = new CombineStateMachine(this);
        SpriteRotator.Register(this);

        if (isApply && TestStats != null)
        {
            Stats.SetCharacterStats(TestStats.BaseHP, TestStats.BaseHP, TestStats.BaseDEF, TestStats.BaseAttackSpeed, TestStats.BaseAttackPower,
                TestStats.BaseCriticalRate, TestStats.BaseCriticalPower, TestStats.BaseMoveSpeed, TestStats.BaseRunMultiplier);
        }
        Health.OnDead += UIController.Instance.UIDie;
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void OnDrawGizmos()
    {
        Ray downRay = new Ray(transform.position, Vector3.down);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(downRay);
    }

    public void PlaySound()
    {
        stateMachine.PlaySound();
    }
}
