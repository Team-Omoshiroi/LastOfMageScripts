using System;
using System.Collections.Generic;

public class PlayerLoginReq
{
    public string PlayerName { get; set; }
}

public class PlayerLoginRes
{
    public bool IsLoginSucceed { get; set; }
    public int PlayerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public PlayerStatRes PlayerStatRes { get; set; }
    public List<PlayerItemRes> Items { get; set; }
}

public class PlayerItemRes
{
    public int TemplateId { get; set; }
    public int Quantity { get; set; }
}

public class InitPlayerStatReq
{
    public int PlayerId { get; set; }
    public int Level { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int Atk { get; set; }
    public float AtkSpeed { get; set; }
    public int CritRate { get; set; }
    public float CritDamage { get; set; }
    public float MoveSpeed { get; set; }
    public float RunMultiplier { get; set; }
    public float DodgeTime { get; set; }
}

public class GetPlayerStatReq
{
    public int PlayerId { get; set; }
}

public class PlayerStatRes
{
    public int PlayerStatId { get; set; }
    public int Level { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int Atk { get; set; }
    public float AtkSpeed { get; set; }
    public int CritRate { get; set; }
    public float CritDamage { get; set; }
    public float MoveSpeed { get; set; }
    public float RunMultiplier { get; set; }
    public float DodgeTime { get; set; }
}