using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[Serializable]
public struct TouchAreaData
{
    public eTouchAreaType AreaType;
    public Vector2 CenterPoint;
    public float Radius;
}

public class PlayerInput : BaseInput, ThirdPersonController.ITestActions, ThirdPersonController.IPlayerActions
{
    public ThirdPersonController InputActions { get; private set; }

#if TEST_PC || UNITY_EDITOR
    public ThirdPersonController.TestActions TestActions { get; private set; }
#else
    public ThirdPersonController.PlayerActions PlayerActions { get; private set; }
#endif
    private CharacterMovement _movement;

    [Header("컨트롤 범위 설정")]
    [SerializeField]
    private List<TouchAreaData> _TouchArea;

    [Header("수동 조준 공격을 위한 최저 누름 시간")]
    [SerializeField]
    private float _aimThresholdTime;

    [Header("회피를 위한 최대 누름 시간")]
    [SerializeField]
    private float _dodgeThresholdTime;
    private bool _isCanControl;
    public bool CanControl
    {
        get => _isCanControl;
        set
        {
            _isCanControl = value;
            if (!value)
            {
                TouchState endTouch = new TouchState()
                {
                    phase = UnityEngine.InputSystem.TouchPhase.Ended,
                    startPosition = Vector3.zero,
                    startTime = Time.realtimeSinceStartup,
                    position = Vector3.zero,
                };
                foreach (var act in _touchAction)
                {
                    CallConnectAction(act.Key, endTouch);
                }
                _touchAction.Clear();
            }
        }
    }

    [SerializeField]
    private TMP_Text MoveTestTxt;

    [SerializeField]
    private TMP_Text AimTestTxt;

    [SerializeField]
    private TMP_Text DodgeTestTxt;

    Dictionary<int, (eTouchAreaType, Action<TouchState>)> _touchAction;
    bool[] _touchAreaOccupied;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();

        _touchAction = new Dictionary<int, (eTouchAreaType, Action<TouchState>)>();

        _touchAreaOccupied = new bool[Enum.GetValues(typeof(eTouchAreaType)).Length];
        for (int i = 0; i < _touchAreaOccupied.Length; i++)
            _touchAreaOccupied[i] = false;

        InputActions = new ThirdPersonController();

#if TEST_PC || UNITY_EDITOR
        TestActions = InputActions.Test;
        TestActions.AddCallbacks(this);
#else
        PlayerActions = InputActions.Player;
        PlayerActions.AddCallbacks(this);
#endif
        if (MoveTestTxt != null)
            OnMoveEvent += (x) =>
            {
                MoveTestTxt.text = x.ToString();
            };
        if (AimTestTxt != null)
        {
            OnAimEvent += (x) =>
            {
                AimTestTxt.text = x.ToString();
            };
            OnAttackEvent += (x) =>
            {
                AimTestTxt.text = "Attack";
            };
        }
        if (DodgeTestTxt != null)
        {
            OnRunEvent += (x) =>
            {
                DodgeTestTxt.text = x.ToString();
            };
            OnDodgeEvent += () =>
            {
                DodgeTestTxt.text = "Dodge";
            };
        }
    }

    private void Reset()
    {
        _touchAction.Clear();
        for (int i = 0; i < _touchAreaOccupied.Length; i++)
            _touchAreaOccupied[i] = false;
    }

    private void OnEnable()
    {
        InputActions.Enable();
        CanControl = true;
    }

    private void OnDisable()
    {
        InputActions.Disable();
        CanControl = false;
    }

    // 터치 조작
    public void OnCancelTouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Application.Quit();
        }
    }

    public void OnMainTouch(InputAction.CallbackContext context)
    {
        if (_isCanControl)
        {
            var touch = context.ReadValue<TouchState>();
            TouchToAction(touch);
        }
    }

    private void TouchToAction(TouchState touch)
    {
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            // 터치 영역을 확인
            eTouchAreaType area = CheckTouchArea(touch.startPosition);
            // 터치 아이디와 행동 함수를 연결
            if (!CheckAreaOccupied(area))
            {
                ConnectToAreaAction(area, touch);
                CallConnectAction(touch.touchId, touch);
            }
        }
        else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            // 터치 아이디에 연결된 행동 함수를 호출
            CallConnectAction(touch.touchId, touch);
        }
        else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            // 터치 아이디에 연결된 행동 함수를 호출
            CallConnectAction(touch.touchId, touch);
            // 연결 삭제
            RemoveAreaAction(touch.touchId);
        }
    }

    private void CallConnectAction(int touchId, TouchState touch)
    {
        if (
            _touchAction.TryGetValue(
                touchId,
                out (eTouchAreaType, Action<TouchState>) connectedTouch
            )
        )
            connectedTouch.Item2?.Invoke(touch);
    }

    private void RemoveAreaAction(int touchId)
    {
        if (_touchAction.ContainsKey(touchId))
        {
            _touchAreaOccupied[(int)_touchAction[touchId].Item1] = false;
            _touchAction.Remove(touchId);
        }
    }

    private void ConnectToAreaAction(eTouchAreaType area, TouchState touch)
    {
        _touchAreaOccupied[(int)area] = true;
        Action<TouchState> action;
        switch (area)
        {
            case eTouchAreaType.Move:
                action = MoveAction;
                break;
            case eTouchAreaType.AimFire:
                action = AimFireAction;
                break;
            case eTouchAreaType.DodgeRun:
                action = DodgeRunAction;
                break;
            default:
                action = null;
                break;
        }
        _touchAction.Add(touch.touchId, (area, action));
    }

    private void MoveAction(TouchState touch)
    {
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            CallMoveEvent(Vector3.zero);
        else
            CallMoveEvent(touch.position - touch.startPosition);
    }

    private void AimFireAction(TouchState touch)
    {
        if (
            touch.phase == UnityEngine.InputSystem.TouchPhase.Began
            || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved
        )
            CallAimEvent(touch.position - touch.startPosition);
        else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            if (Time.realtimeSinceStartup - touch.startTime > _aimThresholdTime)
                CallAttackEvent(touch.position - touch.startPosition);
            else
                CallAttackEvent(AutoTargeting());
        }
    }

    private Vector3 AutoTargeting()
    {
        Vector3 target = Vector3.zero;

        // 가장 가까운 적의 위치를 가지고 온다.

        return target;
    }

    private void DodgeRunAction(TouchState touch)
    {
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            CallRunEvent(false);
            if (Time.realtimeSinceStartup - touch.startTime < _dodgeThresholdTime)
            {
                var area = _TouchArea.Find((x) => x.AreaType == eTouchAreaType.DodgeRun);
                if ((touch.position - area.CenterPoint).magnitude < area.Radius)
                    CallDodgeEvent();
            }
        }
        else
        {
            CallRunEvent(true);
        }
    }

    private bool CheckAreaOccupied(eTouchAreaType area)
    {
        return _touchAreaOccupied[(int)area];
    }

    private eTouchAreaType CheckTouchArea(Vector2 position)
    {
        eTouchAreaType touchArea = eTouchAreaType.None;
        foreach (var area in _TouchArea)
        {
            if ((position - area.CenterPoint).magnitude < area.Radius)
            {
                touchArea = area.AreaType;
                break;
            }
        }
        return touchArea;
    }

    public void OnTapTouch(InputAction.CallbackContext context) { }

    // 키보드 마우스 조작
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            var point = context.ReadValue<Vector2>();
            Vector2 center = new Vector2() { x = Display.main.renderingWidth / 2, y = Display.main.renderingHeight / 2 };
            CallAimEvent(point - center);
        }
        else if (context.phase == InputActionPhase.Canceled)
            CallAimEvent(Vector2.zero);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CallAttackEvent(Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()) - gameObject.transform.position);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //if (context.phase == InputActionPhase.Performed)
        if (context.phase == InputActionPhase.Canceled)
        {
            CallMoveEvent(Vector3.zero);
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            CallMoveEvent(dir);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            CallRunEvent(false);
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            CallRunEvent(true);
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CallDodgeEvent();
        }
    }
}
