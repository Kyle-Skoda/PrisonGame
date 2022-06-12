using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : PlayerComponent
{
    private InputMaster controls;

    public Action OnJump;
    public Action OnPause;
    public Action OnInteract;
    public Action<bool> OnClick;
    public Action<bool> OnProne;
    public Action<bool> OnCrouch;
    public Action<bool> OnSprint;
    public Action<bool> OnScroll;

    public Vector2 MoveInputDirection { get; private set; }
    public Vector2 MouseInputDelta { get; private set; }

    public override void Init()
    {
        base.Init();
        controls = new InputMaster();

        controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.MouseInput.performed += ctx => MouseMove(ctx.ReadValue<Vector2>());
        controls.Player.Pause.started += ctx => OnPause?.Invoke();
        controls.Player.Click.started += ctx => OnClick?.Invoke(true);
        controls.Player.Click.canceled += ctx => OnClick?.Invoke(false);
        controls.Player.Jump.started += ctx => OnJump?.Invoke();
        controls.Player.Interact.started += ctx => OnInteract?.Invoke();
        controls.Player.Crouch.started += ctx => OnCrouch?.Invoke(true);
        controls.Player.Crouch.canceled += ctx => OnCrouch?.Invoke(false);
        controls.Player.Prone.started += ctx => OnProne?.Invoke(true);
        controls.Player.Prone.canceled += ctx => OnProne?.Invoke(false);
        controls.Player.Sprint.started += ctx => OnSprint?.Invoke(true);
        controls.Player.Sprint.canceled += ctx => OnSprint?.Invoke(false);
        controls.Player.Scroll.performed += ctx => Scroll(ctx.ReadValue<float>());
    }

    private void Move(Vector2 direction) => MoveInputDirection = direction;
    private void MouseMove(Vector2 direction) => MouseInputDelta = direction;

    private void Scroll(float val)
    {
        if (val == 0) return;

        if (val > 0)
            OnScroll?.Invoke(true);
        else
            OnScroll?.Invoke(false);
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
}
