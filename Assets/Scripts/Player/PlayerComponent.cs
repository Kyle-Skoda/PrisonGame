using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(PlayerObject))]
public class PlayerComponent : MonoBehaviour
{
    protected PlayerObject player;

    public virtual void Init()
    {
        player = GetComponent<PlayerObject>();

        if (!player.photonView.IsMine)
        {
            player.Rb.isKinematic = true;
            enabled = false;
        }
    }
}
