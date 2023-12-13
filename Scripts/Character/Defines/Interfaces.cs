// 인터페이스 모음
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public interface ILootable
{
    public void Loot();
}

public interface IDroppable
{
    public void Drop();
}

public interface IEquippable
{
    public void Equip(DataContainer dataContainer);
    public void Dequip(DataContainer dataContainer);
}

public interface IConsumable
{
    public void Consume();
}

public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
    public void PhysicsUpdate();
    public void SetAnimation(Animator animator, int layer, float normalizeTime);
    public void PlaySound();
}



public interface IDestroyableItem
{

}

public interface IItemAction
{
    public string ActionName { get; }
    bool PerformAction(GameObject character);

}


