using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _controller;
    private CharacterDataContainer _dataContainer;

    [Header("충격에 관한 설정")]
    [SerializeField][Tooltip("질량.\n 높을수록 충격에 움직이는 거리가 작다.")][Range(0.5f, 2f)] private float _mass = 1.0f;
    [SerializeField][Tooltip("충격시간.\n 높을수록 충격이 오래 지속된다.\n = 충격에 의해 더 멀리 날라간다.")] private float ImpactDampingTime = 0.2f;
    [Header("조작에 관한 설정")]
    [SerializeField][Tooltip("조작 반응성.\n 높을수록 조작이 느리게 반영된다.")] private float ControlDampingTime = 0.2f;
    //[SerializeField][Tooltip("조작으로 인한 최대 속도")] private float MaxSpeed = 0.5f;
    private float MaxSpeed => _dataContainer.Stats.MoveSpeed;
    [Header("중력에 관한 설정")]
    [SerializeField][Tooltip("중력으로 인한 추락 최대 속도")] private float MaxFallSpeed = -3.0f;

    private Vector3 _controlDirection;
    private Vector3 _dampingVelocity;
    private Vector3 _controlDamping;

    private Vector3 _currentPhysics;
    private Vector3 _currentControl;
    private float _currentGravity;

    private float _knockoutTime;

    public float SpeedMultiflier { get; set; } = 1.0f;
    public Vector3 ControlDirection { get => _controlDirection; }
    public Vector3 FinalDirection { get; private set; }
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _dataContainer = GetComponent<CharacterDataContainer>();
    }

    private void Update()
    {
        // 조작 불가 시간
        if (_knockoutTime > 0)
            _knockoutTime -= Time.deltaTime;

        // 캐릭터 이동
        CalControl();
        CalPhysics();
        FinalDirection = (_currentPhysics + _currentControl + _currentGravity * Vector3.up);
        _controller.Move(FinalDirection * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // 중력 계산 => g 가속도. 너무 빨라지지 않도록 clamping 하였음.
        if (!_controller.isGrounded)
            _currentGravity = Mathf.Max(_currentGravity + Physics.gravity.y * Time.fixedDeltaTime, MaxFallSpeed);
        else
            _currentGravity = .0f;
    }

    public void Reset()
    {
        _controlDirection = Vector3.zero;
        _dampingVelocity = Vector3.zero;
        _currentControl = Vector3.zero;
        _currentPhysics = Vector3.zero;
        _currentGravity = 0;
        _knockoutTime = 0;
    }

    /// <summary>
    /// 해당 방향으로 충격/속도을 받는다. 속도 변화에 제한이 없다.
    /// </summary>
    /// <param name="impact">적용 충격/속도</param>
    /// <param name="knockoutDuration">조작 불가능 시간</param>
    public void AddImpact(Vector3 impact, float knockoutDuration = 0.3f)
    {
        Debug.Log("넉백");
        _currentPhysics += impact / _mass;
        ApplyKnockOutTime(knockoutDuration);
    }

    /// <summary>
    /// 조작 불가능 시간을 업데이트하나, 최대값만 적용
    /// </summary>
    /// <param name="knockoutTime">조작 불가능 시간</param>
    private void ApplyKnockOutTime(float knockoutTime)
    {
        _knockoutTime = Mathf.Max(_knockoutTime, knockoutTime);
    }

    /// <summary>
    /// 조작 속도로 최대 제한이 있다.
    /// </summary>
    /// <param name="direction">속도</param>
    public void ControlMove(Vector3 direction)
    {
        _controlDirection = direction;
    }

    /// <summary>
    /// 주어진 값만큼 위치를 변경시키나, Collider가 있으면 막힌다.
    /// </summary>
    /// <param name="deltaPosition">이동하는 벡터값</param>
    public void DeltaMovement(Vector3 deltaPosition)
    {
        _controller.Move(deltaPosition);
    }

    /// <summary>
    /// 주어진 위치로 무조건 위치를 변경시킨다. Collider가 있으면 대참사.
    /// </summary>
    /// <param name="position">world position</param>
    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    private void CalControl()
    {
        if (_controlDirection == Vector3.zero)
            _currentControl = Vector3.SmoothDamp(_currentControl, Vector3.zero, ref _controlDamping, ControlDampingTime);
        else
            _currentControl = Vector3.SmoothDamp(_currentControl, _controlDirection * MaxSpeed * SpeedMultiflier, ref _controlDamping, ControlDampingTime);
    }

    private void CalPhysics()
    {
        // 충격을 부드럽게 줄인다.
        _currentPhysics = Vector3.SmoothDamp(_currentPhysics, Vector3.zero, ref _dampingVelocity, ImpactDampingTime);
    }
}
