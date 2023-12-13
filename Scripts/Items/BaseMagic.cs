using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseMagicSO", menuName = "Item/BaseMagicSO")]
public class BaseMagic : BaseItem, IEquippable
{
    [Header("MagicData")]
    //[SerializeField] private GameObject magicObject;
    [SerializeField] protected float range;
    //[SerializeField] private Animation anime;
    [SerializeField] protected AttackData attackData;

    public void Equip(DataContainer dataContainer)
    {
    }

    public void Dequip(DataContainer dataContainer)
    {
    }

    //public GameObject MagicObject { get { return magicObject; } }
    public float Range { get { return range; } }
    //public Animation Anime { get { return anime; } }
    public AttackData AttackData { get {  return attackData; } }
}
