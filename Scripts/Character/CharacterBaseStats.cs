using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterBaseStatsSO", menuName = "Character/CharacterBaseStatsSO")]
public class CharacterBaseStats : ScriptableObject
{
    [Header("Character's base stat")]
    [SerializeField][Tooltip("기본 체력")] private int _baseHP;
    [SerializeField][Tooltip("기본 방어력")] private int _baseDEF;
    [SerializeField][Tooltip("기본 공격 속도")] private float _baseAttackSpeed;
    [SerializeField][Tooltip("기본 공격력")] private int _baseAttackPower;
    [SerializeField][Tooltip("기본 크리티컬 확률")] private int _baseCriticalRate;
    [SerializeField][Tooltip("기본 크리티컬 시 피해량 배율")] private float _baseCriticalPower;
    [SerializeField][Tooltip("기본 이동 속도")] private float _baseMoveSpeed;
    [SerializeField][Tooltip("달리기 시 기본 이동속도 배율")] private float _baseRunMultiplier;

    public int BaseHP { get => _baseHP; }
    public int BaseDEF { get => _baseDEF; }
    public float BaseAttackSpeed { get => _baseAttackSpeed; }
    public int BaseAttackPower { get => _baseAttackPower; }
    public int BaseCriticalRate { get => _baseCriticalRate; }
    public float BaseCriticalPower { get => _baseCriticalPower; }
    public float BaseMoveSpeed { get => _baseMoveSpeed; }
    public float BaseRunMultiplier { get => _baseRunMultiplier; }
}
