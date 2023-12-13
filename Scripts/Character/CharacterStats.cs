using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public event Action OnHpChange;

    private int level;
    private int maxHp;
    private int hp;
    private int def;
    private int atk;
    private float atkSpeed;
    private int critRate;
    private float critDamage;
    private float moveSpeed;
    private float runMultiplier;
    private float dodgeTime;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public int Hp
    {
        get { return hp; }
        set
        {
            if (value > MaxHp)
                hp = MaxHp;
            else
                hp = value;
            OnHpChange?.Invoke();
        }
    }

    public int Def
    {
        get { return def; }
        set { def = value; }
    }

    public float AtkSpeed
    {
        get { return atkSpeed; }
        set { atkSpeed = value; }
    }

    public int Atk
    {
        get { return atk; }
        set { atk = value; }
    }

    public int CritRate
    {
        get { return critRate; }
        set { critRate = value; }
    }

    public float CritDamage
    {
        get { return critDamage; }
        set { critDamage = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float RunMultiplier
    {
        get { return runMultiplier; }
        set { runMultiplier = value; }
    }

    public float DodgeTime
    {
        get { return dodgeTime; }
        set { dodgeTime = value; }
    }

    public void SetCharacterStats(
        int maxHp,
        int hp,
        int def,
        float atkSpeed,
        int atk,
        int critRate,
        float critDamage,
        float moveSpeed,
        float runMuliplier
    )
    {
        MaxHp = maxHp;
        Hp = hp;
        Def = def;
        AtkSpeed = atkSpeed;
        Atk = atk;
        CritRate = critRate;
        CritDamage = critDamage;
        MoveSpeed = moveSpeed;
        RunMultiplier = runMuliplier;
    }
}
