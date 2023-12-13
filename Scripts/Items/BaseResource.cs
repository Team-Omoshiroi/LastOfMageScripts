using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseResourceSO", menuName = "Item/BaseResourceSO")]
public class BaseResource : BaseItem, IDroppable
{
    protected int _weight;
    protected int _value;

    public int Weight { get => _weight; }
    public int Value { get => _value; }

    public void Drop()
    {
        
    }
}
