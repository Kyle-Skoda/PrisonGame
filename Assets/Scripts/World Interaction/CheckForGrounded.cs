using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGrounded : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Vector3 originOffset = Vector3.zero;
    [SerializeField] private Vector3 halfExtents = Vector3.one;
    [SerializeField] private float distance = 1;

    public bool IsGrounded { get; private set; }

    public Action OnHitGround;

    private void FixedUpdate()
    {
        if (Physics.BoxCast(transform.position + originOffset, halfExtents, Vector3.down, Quaternion.identity, distance, groundLayers))
        {
            if (!IsGrounded)
                OnHitGround?.Invoke();
            IsGrounded = true;
        }
        else
            IsGrounded = false;
    }
}
