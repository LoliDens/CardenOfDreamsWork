using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : IPlayerInputService
{
    private const string ACTION_MAP_NAME = "Player";

    private const string NAME_MOUSE_POSITON = "MousePosition";
    private const string NAME_MOUSE_LEFT_CLICK = "MouseLeftClick";

    private readonly InputActionAsset _playerControls;

    private Action<Vector2> _actionMouseMove;
    private Action _actionMouseLeftClick;

    private InputAction _mouseMoveAction;
    private InputAction _mouseLeftClickAction;

    public Vector2 MouseMove { get; private set; }
    public bool MouseLeftClick { get; private set; }

    public void RegisterActionMouseMove(Action<Vector2> action) =>
        _actionMouseMove += action;

    public void UnregisterActionMouseMove(Action<Vector2> action) =>
       _actionMouseMove -= action;

    public void RegisterActionMouseLeftClick(Action action) =>
     _actionMouseLeftClick += action;

    public void UnregisterActionMouseLeftClick(Action action) =>
       _actionMouseLeftClick -= action;

    public PlayerInput(InputActionAsset inputActions)
    {
        _playerControls = inputActions;

        _mouseMoveAction = _playerControls.FindActionMap(ACTION_MAP_NAME).FindAction(NAME_MOUSE_POSITON);
        _mouseLeftClickAction = _playerControls.FindActionMap(ACTION_MAP_NAME).FindAction(NAME_MOUSE_LEFT_CLICK);

        _mouseMoveAction.Enable();
        _mouseLeftClickAction.Enable();

        RegisterInputAction();
    }

    private void RegisterInputAction()
    {
        _mouseMoveAction.performed += context => MouseMove = context.ReadValue<Vector2>();
        _mouseMoveAction.performed += context => _actionMouseMove?.Invoke(context.ReadValue<Vector2>());
        //_mouseMoveAction.canceled += context => MouseMove = Vector2.zero;

        _mouseLeftClickAction.performed += context => MouseLeftClick = true;
        _mouseLeftClickAction.performed += context => _actionMouseLeftClick?.Invoke();
        _mouseLeftClickAction.canceled += context => MouseLeftClick = false;
    }
}
