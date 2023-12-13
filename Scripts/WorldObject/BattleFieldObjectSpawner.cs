using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추후, 이 기능 전체를 BattleFieldObjectManager 에 통합시키는 것이 좋아 보인다.
/// </summary>
public class BattleFieldObjectSpawner : MonoBehaviour
{
    public static BattleFieldObjectSpawner instance;
    public GameObject farmingBox;

    private void Awake()
    {
        if (instance == null) { instance = this; }    
    }

    public void SpawnFarmingBox(int objectId, Vector3 pos)
    {
        GameObject fb = Instantiate(farmingBox, pos, Quaternion.Euler(0,0,0));
        BattleFieldObject bfo = fb.GetComponent<BattleFieldObject>();
        bfo.ObjectId = objectId;
        fb.SetActive(true);
    }
}
