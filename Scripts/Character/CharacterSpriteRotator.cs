using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CharacterSpriteRotator
{
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _weapon;
    private Vector3 _weaponBase;

    public void Register(CharacterDataContainer container)
    {
        container.InputActions.OnMoveEvent += MoveInput;
        container.InputActions.OnAimEvent += AimInput;
        _weaponBase = _weapon.transform.position;
    }

    public void Register(CloneDataContainer container)
    {
        container.Sync.OnAimEvent += AimNetwork;
        container.Sync.OnMoveEvent += MoveNetwork;
        _weaponBase = _weapon.transform.position;
    }

    public void Register(NPCDataContainer container)
    {
        _weaponBase = _weapon.transform.position;
        // TODO
        // Get Information about direcction or update 
        //Vector3 f = Camera.main.transform.forward;
        //Vector3 r = Camera.main.transform.right;
        // What's value of monster front?

        container.InputActions.OnAimEvent += AimInput;
        container.InputActions.OnMoveEvent += MoveInput;
    }
    private void MoveInput(Vector2 direction)
    {
        RotateMain(direction);
    }

    private void AimInput(Vector2 direction)
    {
        RotateWeapon(direction);
    }

    private void MoveNetwork(int state, Vector3 posInfo, Vector3 velInfo)
    {
        RotateMain(velInfo);
    }

    private void AimNetwork(int state, Vector3 velInfo)
    {
        RotateWeapon(velInfo);
    }

    private void RotateMain(Vector3 direction)
    {
        float direc = direction.x * _main.transform.localScale.x;
        if (direc < 0)
        {
            Vector3 target = _main.transform.localScale;
            target.x = target.x * (-1);
            _main.transform.localScale = target;
        }
    }

    private void RotateWeapon(Vector3 direction)
    {
        Vector3 axis;
        if (direction.x < 0)
        {
            _weapon.transform.localScale = new Vector3() { x = -1, y = 1, z = 1 };
            axis = new Vector3() { x = -1, y = 0, z = 0 };

            float angle = Vector3.Angle(axis, direction);
            if (direction.z > 0)
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = -angle };
            else
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = angle };
        }
        else
        {
            _weapon.transform.localScale = Vector3.one;
            axis = new Vector3() { x = 1, y = 0, z = 0 };

            float angle = Vector3.Angle(axis, direction);
            if (direction.z < 0)
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = -angle };
            else
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = angle };
        }
    }

    private void RotateMain(Vector2 direction)
    {
        float direc = direction.x * _main.transform.localScale.x;
        if (direc < 0)
        {
            Vector3 target = _main.transform.localScale;
            target.x = target.x * (-1);
            _main.transform.localScale = target;
        }
    }

    private void RotateWeapon(Vector2 direction)
    {
        Vector2 axis;
        if (direction.x < 0)
        {
            _weapon.transform.localScale = new Vector3() { x = -1, y = 1, z = 1 };
            axis = new Vector2() { x = -1, y = 0 };

            float angle = Vector2.Angle(axis, direction);
            if (direction.y > 0)
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = -angle };
            else
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = angle };
        }
        else
        {
            _weapon.transform.localScale = Vector3.one;
            axis = new Vector2() { x = 1, y = 0 };

            float angle = Vector2.Angle(axis, direction);
            if (direction.y < 0)
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = -angle };
            else
                _weapon.transform.localEulerAngles = new Vector3() { x = 90, y = 0, z = angle };
        }
    }
}
