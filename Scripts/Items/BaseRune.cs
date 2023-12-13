using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BaseRuneSO", menuName = "Item/BaseRuneSO")]
public class BaseRune : BaseItem, IDroppable, IEquippable
{
    public void Drop()
    {

    }

    public void Equip(DataContainer dataContainer)
    {
    }

    public void Dequip(DataContainer dataContainer)
    {
    }
}
