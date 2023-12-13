using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldObject : MonoBehaviour
{
    private int objectId = 0;

    /// <summary>
    /// 추후, set 과정에서 value 의 중복 여부를 판단하는 내용이 들어갈 것이다.
    /// </summary>
    public int ObjectId 
    {
        get { return objectId; }
        set { objectId = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
