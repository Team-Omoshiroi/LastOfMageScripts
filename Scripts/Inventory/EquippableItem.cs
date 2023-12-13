using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EquippableItem : BaseItem, IDestroyableItem, IItemAction, IEquippable
{
    public string ActionName => "Equip";

    public void Dequip(DataContainer dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public void Equip(DataContainer dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public bool PerformAction(GameObject character)
    {
       
        Debug.Log("장착");
        return true;
    }
}
