using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckForGrounded))]
public class PlayerMovement : PlayerComponent
{
    public enum PlayerMoveState
    {
        Idle = 0,
        Walking = 1,
        Sprinting = 2,
        Crouching = 3,
        Prone = 4
    }

    public bool ToggleCrouch = false;
    public bool ToggleSprint = false;
    public bool ToggleProne = false;

    [SerializeField] private float maxWalkSpeed = 15;
    [SerializeField] private float sprintMultiplier = 2;
    [SerializeField] private float crouchMultiplier = 0.6f;
    [SerializeField] private float proneMultiplier = 0.4f;
    [SerializeField] private float maxStamina = 5;
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 15;
    [SerializeField] private int maxJumpCount = 1;

    private float currentStamina = 0;
    private float currentJumpCount = 0;
    private float currentMoveSpeedMultiplier = 1;
    private PlayerMoveState currentMoveState = PlayerMoveState.Idle;
    private Coroutine sprintRoutine;

    public override void Init()
    {
        base.Init();
        currentStamina = maxStamina;
        currentJumpCount = maxJumpCount;
        player.GroundCheck.OnHitGround += OnHitGround;
        player.Input.OnCrouch += OnCrouch;
        player.Input.OnSprint += OnSprint;
        player.Input.OnProne += OnProne;
        player.Input.OnJump += OnJump;
    }

    private void OnDestroy()
    {
        player.GroundCheck.OnHitGround -= OnHitGround;
        player.Input.OnCrouch -= OnCrouch;
        player.Input.OnSprint -= OnSprint;
        player.Input.OnProne -= OnProne;
        player.Input.OnJump -= OnJump;
    }

    private void OnHitGround() => currentJumpCount = maxJumpCount;

    private void OnJump()
    {
        if (currentJumpCount <= 0 || !player.GroundCheck.IsGrounded) return;
        currentJumpCount--;
        if (currentMoveState != PlayerMoveState.Idle || currentMoveState != PlayerMoveState.Walking || currentMoveState != PlayerMoveState.Sprinting)
        {
            if (player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (player.Input.MoveInputDirection != Vector2.zero && currentMoveState == PlayerMoveState.Idle)
                currentMoveState = PlayerMoveState.Walking;

            player.Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCrouch(bool crouch)
    {
        if (ToggleCrouch)
        {
            if (crouch == false) return;

            if (currentMoveState != PlayerMoveState.Crouching)
                currentMoveState = PlayerMoveState.Crouching;
            else if (currentMoveState == PlayerMoveState.Crouching && player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (currentMoveState == PlayerMoveState.Crouching && player.Input.MoveInputDirection != Vector2.zero)
                currentMoveState = PlayerMoveState.Walking;
        }
        else
        {
            if (crouch)
                currentMoveState = PlayerMoveState.Crouching;
            else if (!crouch && player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (!crouch && player.Input.MoveInputDirection != Vector2.zero)
                currentMoveState = PlayerMoveState.Walking;
        }
    }

    private void OnProne(bool prone)
    {
        if (ToggleProne)
        {
            if (prone == false) return;

            if (currentMoveState != PlayerMoveState.Prone)
                currentMoveState = PlayerMoveState.Prone;
            else if (currentMoveState == PlayerMoveState.Prone && player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (currentMoveState == PlayerMoveState.Prone && player.Input.MoveInputDirection != Vector2.zero)
                currentMoveState = PlayerMoveState.Walking;
        }
        else
        {
            if (prone)
                currentMoveState = PlayerMoveState.Prone;
            else if (!prone && player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (!prone && player.Input.MoveInputDirection != Vector2.zero)
                currentMoveState = PlayerMoveState.Walking;
        }
    }

    private void OnSprint(bool sprint)
    {
        //TODO: Add sprint coroutine for stamina
        if (ToggleSprint)
        {
            if (sprint == false) return;

            if (currentMoveState != PlayerMoveState.Sprinting)
                currentMoveState = PlayerMoveState.Sprinting;
            else if (currentMoveState == PlayerMoveState.Sprinting && player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (currentMoveState == PlayerMoveState.Sprinting && player.Input.MoveInputDirection != Vector2.zero)
                currentMoveState = PlayerMoveState.Walking;
        }
        else
        {
            if (sprint)
                currentMoveState = PlayerMoveState.Sprinting;
            else if (!sprint && player.Input.MoveInputDirection == Vector2.zero)
                currentMoveState = PlayerMoveState.Idle;
            else if (!sprint && player.Input.MoveInputDirection != Vector2.zero)
                currentMoveState = PlayerMoveState.Walking;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.right * player.Input.MoveInputDirection.x + transform.forward * player.Input.MoveInputDirection.y;

        // if (player.Input.MoveInputDirection == Vector2.zero)
        //     currentMoveState = PlayerMoveState.Idle;
        // else if (player.Input.MoveInputDirection != Vector2.zero && currentMoveState == PlayerMoveState.Idle)
        //     currentMoveState = PlayerMoveState.Walking;

        switch (currentMoveState)
        {
            case (PlayerMoveState.Crouching):
                currentMoveSpeedMultiplier = crouchMultiplier;
                transform.localScale = Vector3.one * 0.5f;
                break;
            case (PlayerMoveState.Prone):
                currentMoveSpeedMultiplier = proneMultiplier;
                transform.localScale = Vector3.one * 0.25f;
                break;
            case (PlayerMoveState.Sprinting):
                currentMoveSpeedMultiplier = sprintMultiplier;
                transform.localScale = Vector3.one;
                break;
            default:
                currentMoveSpeedMultiplier = 1;
                transform.localScale = Vector3.one;
                break;
        }

        moveDirection = moveDirection.normalized * (maxWalkSpeed * currentMoveSpeedMultiplier * Time.fixedDeltaTime);
        moveDirection.y = player.Rb.velocity.y;

        player.Rb.velocity = moveDirection;
    }
}