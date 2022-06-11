using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> disableRenderForLocalPlayer;

    // private List<PlayerComponent> components;

    [HideInInspector] public Rigidbody Rb;
    [HideInInspector] public PlayerInputs Input;
    [HideInInspector] public PhotonView photonView;
    [HideInInspector] public CheckForGrounded GroundCheck;
    [HideInInspector] public Camera cam;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInputs>();
        photonView = GetComponent<PhotonView>();
        GroundCheck = GetComponent<CheckForGrounded>();

        if (photonView.IsMine)
            for (int i = 0; i < disableRenderForLocalPlayer.Count; i++)
                disableRenderForLocalPlayer[i].layer = 11;

        foreach (var c in GetComponents<PlayerComponent>())
        {
            c.Init();
            // components.Add(c);

            if (c is CameraRotation)
                cam = c.GetComponent<CameraRotation>().GetCamera();
        }
    }
}
