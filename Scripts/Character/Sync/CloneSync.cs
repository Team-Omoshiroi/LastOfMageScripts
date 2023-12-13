using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class CloneSync : SyncModule
{
    public event Action<int, Vector3, Vector3> OnMoveEvent;
    public event Action<int, Vector3> OnAimEvent;
    public event Action<int, float, Vector3, Vector3> OnBattleEvent;
    public event Action<int, Vector3, Vector3> OnMakeAttackAreaEvent;
    public event Action<int, Vector3, Vector3> OnComboAttackEvent;
    public event Action<Vector3, Vector3> OnDodgeEvent;

    protected override void Update()
    {
        base.Update();
    }

    public void CallMoveEvent(int state, PositionInfo posInfo, VelocityInfo velInfo)
    {
        OnMoveEvent?.Invoke(
            state,
            ToVector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ),
            ToVector3(velInfo.VelX, velInfo.VelY, velInfo.VelZ)
        );
    }

    public void CallAimEvent(int state, VelocityInfo velInfo)
    {
        OnAimEvent?.Invoke(state, ToVector3(velInfo.VelX, velInfo.VelY, velInfo.VelZ));
    }

    public void CallBattleEvent(
        int state,
        float animTime,
        PositionInfo posInfo,
        VelocityInfo velInfo
    )
    {
        OnBattleEvent?.Invoke(
            state,
            animTime,
            ToVector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ),
            ToVector3(velInfo.VelX, velInfo.VelY, velInfo.VelZ)
        );
    }

    // TODO : Protocol 수정 이후 CallBattleEvent 삭제
    public void CallComboAttackEvent(int comboindex, PositionInfo posInfo, DirectionInfo dirInfo)
    {
        OnComboAttackEvent?.Invoke(
            ComboIndex,
            ToVector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ),
            ToVector3(dirInfo.DirX, dirInfo.DirY, dirInfo.DirZ)
        );
    }

    public void CallDodgeEvent(PositionInfo posInfo, VelocityInfo velInfo)
    {
        OnDodgeEvent?.Invoke(
            ToVector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ),
            ToVector3(velInfo.VelX, velInfo.VelY, velInfo.VelZ)
        );
    }

    public void CallMakeAttackAreaEvent(int comboIndex, PositionInfo posInfo, VelocityInfo velInfo)
    {
        OnMakeAttackAreaEvent?.Invoke(
            comboIndex,
            ToVector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ),
            ToVector3(velInfo.VelX, velInfo.VelY, velInfo.VelZ)
        );
        Debug.Log($"Clone Attack ({comboIndex}) ({velInfo.VelX},{velInfo.VelY},{velInfo.VelZ})");
    }
}
