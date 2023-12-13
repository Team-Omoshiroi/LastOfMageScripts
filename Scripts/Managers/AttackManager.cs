using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class PoolObj
{
    [field: SerializeField] public eAttackType Type { get; private set; }
    public GameObject Prefab;
    public int PoolSize;
}

public class AttackManager : CustomSingleton<AttackManager>
{
    // 오브젝트 풀을 위한 큐
    private Dictionary<eAttackType, Queue<GameObject>> PoolsDic = new Dictionary<eAttackType, Queue<GameObject>>();
    //private Queue<GameObject> attackPool = new Queue<GameObject>();

    // 원거리 공격 프리팹
    [SerializeField] private List<PoolObj> PoolItem = new List<PoolObj>();
    //private PoolObj RangeAttackPrefab;
    //private PoolObj MeleeAttackPrefab;
    // 풀 크기 
    //[SerializeField]
    //private int poolSize = 10;
    //풀 
    [SerializeField]
    private GameObject ObjPoolRoot;
    public LayerMask TargetLayer;
    public string PlayerTag;

    public void RqAttack(int ComboIndex, DataContainer dataContainer, Vector3 position, Vector3 direction)
    {
        // 오브젝트풀링
        BaseItem item = dataContainer.Equipments.GetEquippedItem(eItemType.Magic);
        if (item is BaseMagic magic)
        {
            BaseAttack attackObj = GetAttackFromPool(magic.AttackData.AttackInfos[ComboIndex].AttackType);
            // 풀링한오브젝트를 초기화
            attackObj.Initalize(magic.AttackData.AttackInfos[ComboIndex], dataContainer, dataContainer.tag);
            //attackObj.transform.SetLocalPositionAndRotation(position, Quaternion.FromToRotation(attackObj.transform.forward, direction));
            attackObj.transform.SetPositionAndRotation(position, Quaternion.FromToRotation(Vector3.forward, direction));

            // attackObj.transform.rotation을 direction에 따라서 돌리기
        }
    }

    private void Awake()
    {
        InitializePool(); // 오브젝트 풀 초기화
    }

    private void InitializePool()
    {
        // 풀 크기만큼 오브젝트를 생성하여 비활성화 상태로 큐에 추가
        foreach (var pool in PoolItem)
        {
            Queue<GameObject> attackPool = new Queue<GameObject>();
            for (int i = 0; i < pool.PoolSize; i++)
            {
                var newAttack = Instantiate(pool.Prefab, ObjPoolRoot.transform);
                newAttack.SetActive(false);
                attackPool.Enqueue(newAttack);
            }
            PoolsDic.Add(pool.Type, attackPool);
        }
        //for (int i = 0; i < poolSize; i++)
        //{
        //    var newAttack = Instantiate(attackPrefab);
        //    newAttack.SetActive(false);
        //    attackPool.Enqueue(newAttack);
        //}

    }

    public BaseAttack GetAttackFromPool(eAttackType type)
    {
        // 풀에 오브젝트가 있으면 하나를 꺼내서 활성화하고 반환
        if (PoolsDic.ContainsKey(type))
        {
            var attackQueue = PoolsDic[type];
            if (attackQueue.Count > 0)
            {
                var attack = attackQueue.Dequeue();
                if (attack.TryGetComponent<BaseAttack>(out BaseAttack script))
                {
                    attack.SetActive(true);
                    script.AddActAtDisable(() => { ReturnAttackToPool(attackQueue, attack); });
                    return script;
                }
                else
                {
                    attackQueue.Enqueue(attack);
                    return null;
                }
            }
            else
            {
                PoolObj item = PoolItem.Find(x => x.Type == type);
                // 풀이 비어있으면 새로운 오브젝트를 생성하여 반환
                var newAttack = Instantiate(item.Prefab, ObjPoolRoot.transform);
                if (newAttack.TryGetComponent<BaseAttack>(out BaseAttack script))
                {
                    newAttack.SetActive(true);
                    script.AddActAtDisable(() => { ReturnAttackToPool(attackQueue, newAttack); });
                    return script;
                }
                else
                {
                    attackQueue.Enqueue(newAttack);
                    return null;
                }
            }
        }
        else
            return null;
    }

    private void ReturnAttackToPool(Queue<GameObject> queue, GameObject attack)
    {
        // 오브젝트를 비활성화하고 다시 풀에 반환
        attack.gameObject.SetActive(false);
        queue.Enqueue(attack);
    }

    private void LateUpdate()
    {
        // 10분동안 봤는데 Melee의 총 개수는 20개인데, 최대 사용개수가 5개밖에 안되
        // 줄여야겟다. 10개로 줄여
        // Melee의 안쓰는 Queue에 있는 오브젝트를 디스트로이
    }

}
