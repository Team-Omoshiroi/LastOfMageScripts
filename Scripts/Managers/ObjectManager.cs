using Google.Protobuf.Protocol;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : CustomSingleton<ObjectManager>
{
    public PilotSync pilotSync { get; set; }
    public CharacterStats characterStats { get; set; }
    readonly Dictionary<int, GameObject> _objects = new();

    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public void Add(ObjectInfo info, bool pilotPlayer = false)
    {
        GameObjectType objectType = GetObjectTypeById(info.ObjectId);

        if (objectType == GameObjectType.Player)
        {
            Vector3 S_Position = new(info.PosInfo.PosX, info.PosInfo.PosY, info.PosInfo.PosZ);

            if (pilotPlayer)
            {
                GameObject gameObject = Instantiate(
                    Resources.Load<GameObject>("Prefabs/PilotPlayer")
                );

                gameObject.name = info.Name;
                _objects.Add(info.ObjectId, gameObject);

                // TODO
                // 추후에 RestAPI 통신을 통해 받아온 스탯 (Clone과 공유하지 않는)과
                // Packet 통신으로 받아온 스탯 (Clone과 공유하는) 을 결합할 예정

                pilotSync = gameObject.GetComponent<PilotSync>();
                pilotSync.Id = info.ObjectId;
                pilotSync.Name = info.Name;
                pilotSync.PosInfo = info.PosInfo;
                pilotSync.StatInfo = info.StatInfo;
                pilotSync.State = info.State;

                // CharacterStats를 정상적으로 찾지 못함
                // pilotSync.InitCharacterStats()를 찾을 수 있도록 해야 함
                characterStats = gameObject.GetComponent<DataContainer>().Stats;
                characterStats.Level = info.StatInfo.Level;
                characterStats.MaxHp = info.StatInfo.MaxHp;
                characterStats.Hp = info.StatInfo.Hp;
                characterStats.Def = 1;
                characterStats.AtkSpeed = 1;
                characterStats.Atk = info.StatInfo.Attack;
                characterStats.CritRate = 1;
                characterStats.CritDamage = 1;
                characterStats.MoveSpeed = info.StatInfo.Speed;
                characterStats.RunMultiplier = 2;
            }
            else
            {
                Vector3 SpawnPos = S_Position;

                GameObject gameObject = Instantiate(
                    Resources.Load<GameObject>("Prefabs/ClonePlayer"),
                    SpawnPos,
                    Quaternion.identity
                );

                _objects.Add(info.ObjectId, gameObject);

                // TODO
                // Packet 통신으로 받아온 스탯만 결합할 예정
                CloneSync cloneSync = gameObject.GetComponent<CloneSync>();
                cloneSync.Id = info.ObjectId;
                cloneSync.Name = info.Name;
                cloneSync.PosInfo = info.PosInfo;
                cloneSync.StatInfo = info.StatInfo;
                cloneSync.State = info.State;

                // CharacterStats를 정상적으로 찾지 못함
                // pilotSync.InitCharacterStats()를 찾을 수 있도록 해야 함
                characterStats = gameObject.GetComponent<DataContainer>().Stats;
                characterStats.Level = info.StatInfo.Level;
                characterStats.MaxHp = info.StatInfo.MaxHp;
                characterStats.Hp = info.StatInfo.Hp;
                characterStats.Def = 1;
                characterStats.AtkSpeed = 1;
                characterStats.Atk = info.StatInfo.Attack;
                characterStats.CritRate = 1;
                characterStats.CritDamage = 1;
                characterStats.MoveSpeed = info.StatInfo.Speed;
                characterStats.RunMultiplier = 2;
                cloneSync.CallMoveEvent(cloneSync.State, cloneSync.PosInfo, cloneSync.VelInfo);
            }
        }
    }

    public void Remove(int id)
    {
        GameObject gameObject = FindById(id);
        if (gameObject == null)
            return;

        _objects.Remove(id);
        Destroy(gameObject);
    }

    /// <summary>
    /// 클라이언트의 경우, 자체적인 ObjectId 를 탐색하는 것이 아니라 서버 측에서 받아온 ObjectId 를 저장한 
    /// BattleFieldObject.ObjectId 를 탐색하도록 수정할 필요가 있어 보인다.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject FindById(int id)
    {
        _objects.TryGetValue(id, out GameObject gameObject);
        return gameObject;
    }

    public void Clear()
    {
        foreach (GameObject gameObject in _objects.Values)
        {
            Destroy(gameObject);
        }

        _objects.Clear();
        pilotSync = null;
    }
}
