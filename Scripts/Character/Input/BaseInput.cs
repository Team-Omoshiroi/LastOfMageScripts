using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInput : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<bool> OnRunEvent;
    public event Action OnDodgeEvent;
    public event Action<Vector2> OnAimEvent;
    public event Action<Vector2> OnAttackEvent;
    public event Action OpenInventory;
    public event Action CloseInventory;
    public event Action OnAttackAreaMake;

    public void CallOnAttackAreaMake()
    {
        OnAttackAreaMake?.Invoke();
    }
    public void CallMoveEvent(Vector2 input)
    {
        OnMoveEvent?.Invoke(input);
    }

    public void CallRunEvent(bool state)
    {
        OnRunEvent?.Invoke(state);
    }

    public void CallDodgeEvent()
    {
        OnDodgeEvent?.Invoke();
    }

    public void CallAimEvent(Vector2 input)
    {
        OnAimEvent?.Invoke(input);
    }

    public void CallAttackEvent(Vector2 input)
    {
        OnAttackEvent?.Invoke(input);
    }

    public void CallOpenInven()
    {
        OpenInventory?.Invoke();
    }

    public void CallCloseInven()
    {
        CloseInventory?.Invoke();
    }
}
