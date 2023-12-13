using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }
    public float Offset;
    public Vector3 Rotate;
    [Range(0.5f, 20f)] public float Speed;
    private Vector3 _dampingVelocity;

    private Transform _target;

    private Vector3 _targetPos;
    private Vector3 _offset;

    private void Awake()
    {
        Instance = this;
    }

    public void AttachToPlayer(Transform player)
    {
        _target = player;

        Quaternion quaternion = Quaternion.Euler(Rotate.x, Rotate.y, Rotate.z);
        _offset = new Vector3() { x = 0, y = Offset * Mathf.Sin(Rotate.x * Mathf.PI / 180), z = -Offset * Mathf.Cos(Rotate.x * Mathf.PI / 180) };
        _targetPos = _target.position + _offset;
        transform.SetPositionAndRotation(_targetPos, quaternion);
    }

    void Update()
    {
        if (_target != null)
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _dampingVelocity, 1 / Speed);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
