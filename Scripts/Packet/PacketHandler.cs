using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public partial class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;

        ObjectManager.Instance.Add(enterGamePacket.Player, pilotPlayer: true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

        int pilotPlayerId = ObjectManager.Instance.pilotSync.Id;
        if (pilotPlayerId != leaveGamePacket.PlayerId)
            return;

        ObjectManager.Instance.Clear();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (ObjectInfo obj in spawnPacket.Objects)
            ObjectManager.Instance.Add(obj, pilotPlayer: false);
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;

        foreach (int playerId in despawnPacket.ObjectIds)
            ObjectManager.Instance.Remove(playerId);
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        GameObject gameObject = ObjectManager.Instance.FindById(movePacket.ObjectId);

        if (gameObject == null)
            return;

        if (ObjectManager.Instance.pilotSync.Id == movePacket.ObjectId)
            return;

        if (!gameObject.TryGetComponent<CloneSync>(out var cloneSync))
            return;

        // cloneSync.State = movePacket.State;
        // cloneSync.PosInfo = movePacket.PosInfo;
        // cloneSync.VelInfo = movePacket.VelInfo;

        cloneSync.CallMoveEvent(movePacket.State, movePacket.PosInfo, movePacket.VelInfo);
    }

    /// <summary>
    /// 서버에서 받아온 [오브젝트 ID, 최대 체력과 회복 직전 현재 체력, 체력 변동량] 데이터를 이용해 작업을 수행하는 핸들러.
    /// 해당 오브젝트를 식별하고, 그 오브젝트의 체력 상황을 데이터대로 갱신해주면 된다.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changeHpPacket = packet as S_ChangeHp;

        GameObject gameObject = ObjectManager.Instance.FindById(changeHpPacket.ObjectId);

        if (gameObject == null)
        {
            return;
        }
        if (ObjectManager.Instance.pilotSync.Id == changeHpPacket.ObjectId)
        {
            return;
        }

        SyncModule syncModule = gameObject.GetComponent<SyncModule>();

        syncModule.StatInfo.Hp = changeHpPacket.CurrentHp;
    }

    public static void S_AimHandler(PacketSession session, IMessage packet)
    {
        S_Aim aimPacket = packet as S_Aim;

        GameObject gameObject = ObjectManager.Instance.FindById(aimPacket.ObjectId);

        if (gameObject == null)
            return;

        if (ObjectManager.Instance.pilotSync.Id == aimPacket.ObjectId)
            return;

        if (!gameObject.TryGetComponent<CloneSync>(out var cloneSync))
            return;

        cloneSync.CallAimEvent(aimPacket.State, aimPacket.VelInfo);
    }

    public static void S_ComboAttackHandler(PacketSession session, IMessage packet)
    {
        S_ComboAttack comboAttackPacket = packet as S_ComboAttack;

        GameObject gameObject = ObjectManager.Instance.FindById(comboAttackPacket.ObjectId);

        if (gameObject == null)
        {
            return;
        }

        if (ObjectManager.Instance.pilotSync.Id == comboAttackPacket.ObjectId)
        {
            return;
        }

        if (!gameObject.TryGetComponent<CloneSync>(out var cloneSync))
            return;

        cloneSync.CallComboAttackEvent(
            comboAttackPacket.ComboIndex,
            comboAttackPacket.PosInfo,
            comboAttackPacket.DirInfo
        );
    }

    public static void S_DodgeHandler(PacketSession session, IMessage packet)
    {
        S_Dodge dodgePacket = packet as S_Dodge;

        GameObject gameObject = ObjectManager.Instance.FindById(dodgePacket.ObjectId);

        if (gameObject == null)
        {
            return;
        }
        if (ObjectManager.Instance.pilotSync.Id == dodgePacket.ObjectId)
        {
            return;
        }

        if (!gameObject.TryGetComponent<CloneSync>(out var cloneSync))
            return;

        cloneSync.CallDodgeEvent(dodgePacket.PosInfo, dodgePacket.VelInfo);
    }

    public static void S_MakeAttackAreaHandler(PacketSession session, IMessage packet)
    {
        S_MakeAttackArea makeAttackAreaPacket = packet as S_MakeAttackArea;

        GameObject gameObject = ObjectManager.Instance.FindById(makeAttackAreaPacket.ObjectId);

        if (gameObject == null)
        {
            return;
        }
        if (ObjectManager.Instance.pilotSync.Id == makeAttackAreaPacket.ObjectId)
        {
            return;
        }

        if (!gameObject.TryGetComponent<CloneSync>(out var cloneSync))
            return;

        cloneSync.CallMakeAttackAreaEvent(
            makeAttackAreaPacket.ComboIndex,
            makeAttackAreaPacket.PosInfo,
            makeAttackAreaPacket.VelInfo
        );
    }

    public static void S_DieHandler(PacketSession session, IMessage packet) { }

    // /// <summary>
    // /// 클라이언트 측의 FarmingBox 인벤토리 데이터 요청에 대한 응답을 처리하는 Handler.
    // /// 받아온 인벤토리 데이터를 토대로 해당 object ID 를 가지는 FarmingBox 인벤토리의 데이터를 갱신한다.
    // /// </summary>
    // /// <param name="packet">FarmingBox 의 object ID, Dictionary<int, InventoryItem> 을 포함한다.</param>
    // public static void S_FarmingBoxOpenHandler(PacketSession session, IMessage packet) { }

    // /// <summary>
    // /// 서버 측이 클라이언트에서 받아온 FarmingBox 인벤토리 데이터에 대한 응답을 처리하는 Handler.
    // /// 받아오는 별다른 내용이 없으므로, 딱히 처리할 내용도 없을 듯 하다.
    // /// </summary>
    // public static void S_FarmingBoxCloseHandler(PacketSession session, IMessage packet) { }

    public static void S_PingHandler(PacketSession session, IMessage packet)
    {
        C_Pong pongPacket = new C_Pong();
        NetworkManager.Instance.Send(pongPacket);
    }
}
