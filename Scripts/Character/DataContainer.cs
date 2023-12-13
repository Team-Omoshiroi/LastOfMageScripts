using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CharacterDataContainer, CloneDataContainer 의 공통된 부분을 모아 둔 부모 클래스
/// </summary>
public class DataContainer : MonoBehaviour
{
    public Inventory1 Inven { get; }

    [field: SerializeField]
    public CharacterStats Stats { get; protected set; }

    [field: SerializeField]
    public EquipSystem Equipments { get; protected set; }
    public Animator Animator { get; protected set; }

    [field: SerializeField]
    public CharacterAnimationData AnimationData { get; protected set; }

    [field: SerializeField]
    public CharacterSpriteRotator SpriteRotator { get; protected set; }

    //[field: SerializeField]
    public HealthSystem Health { get; protected set; }

    //[field: SerializeField]
    public SyncModule SyncModule { get; protected set; }

    public static Transform AudioListener { get; protected set; }
}
