using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public class NPCDataContainer : DataContainer
{
    public CharacterController Controller { get; private set; }
    [Header("테스트용 몬스터 스탯 및 장비")]
    [SerializeField] private CharacterBaseStats _testMonsterStats;
    [SerializeField] private List<BaseItem> _testMonsterEquipments;
    [Header("테스트용 몬스터 회피")]
    [SerializeField] private bool SetDodge;
    [SerializeField] public GameObject dropItem;

    private NPCAIController npcAIController;

    private CombineStateMachine stateMachine;

    private bool isDead = false;

    public NPCInput InputActions { get; private set; }

    /// <summary>
    /// CombineStateMachine 을 그대로 사용하되, Input 에 대한 사항만 바꾸어주면 된다.
    /// 그리고 AI 의 행동 양식(배회, 추적, 도망, 경계, 대기 등) 에 대한 사항은 별도의 로직으로 관리하면서 
    /// 이에 따라 StateMachine 을 조작하도록 하면 될 것이다.
    /// </summary>
    public NPCAIController NPCAI { get { return npcAIController; } private set { npcAIController = value; } }
    public CombineStateMachine StateMachine { get { return stateMachine; } private set { stateMachine = value; } }

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        Health = GetComponent<HealthSystem>();
        npcAIController = GetComponent<NPCAIController>();
        InputActions = GetComponent<NPCInput>();


        AnimationData.Initialize();
        stateMachine = new CombineStateMachine(this);

        // 추후에 서버로 AI를 옮기면 SyncModule에 대한 참조도 필요함.

        if (_testMonsterStats != null)
            Stats.SetCharacterStats(_testMonsterStats.BaseHP,
                _testMonsterStats.BaseHP,
                _testMonsterStats.BaseDEF,
                _testMonsterStats.BaseAttackSpeed,
                _testMonsterStats.BaseAttackPower,
                _testMonsterStats.BaseCriticalRate,
                _testMonsterStats.BaseCriticalPower,
                _testMonsterStats.BaseMoveSpeed,
                _testMonsterStats.BaseRunMultiplier);

        Equipments = new EquipSystem(this);
        if (_testMonsterEquipments != null)
        {
            foreach (var item in _testMonsterEquipments)
                Equipments.Equip(item);
        }

        SpriteRotator.Register(this);
        Health.OnDead += () => { isDead = true; BattleFieldObjectSpawner.instance.SpawnFarmingBox(0, this.transform.position); Debug.Log("사망 상태. 더이상 상태를 갱신하지 않음."); NPCAI.OnDead(); };

        Health.IsDodge = SetDodge;
    }

    private void Update()
    {
        stateMachine.Update();

        if (!isDead)
        {
            NPCAI?.UpdateAIState();
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void OnEnable()
    {
        isDead = false;
        Debug.Log("부활. 다시 상태를 갱신함.");
    }

    //Vector3 GetFleeLocation()
    //{
    //    NavMeshHit hit;

    //    NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

    //    int i = 0;
    //    while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
    //    {

    //        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
    //        i++;
    //        if (i == 30)
    //            break;
    //    }

    //    return hit.position;
    //}

    //float GetDestinationAngle(Vector3 targetPos)
    //{
    //    return Vector3.Angle(transform.position - PlayerController.instance.transform.position, transform.position + targetPos);
    //}

    public void PlaySound()
    {
        stateMachine.PlaySound();
    }
}
