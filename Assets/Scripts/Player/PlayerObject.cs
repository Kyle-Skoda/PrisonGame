using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> disableRenderForLocalPlayer;
    [SerializeField] private GameObject uiPrefab;

    // private List<PlayerComponent> components;

    public Renderer playerMesh;
    public Material guardMat;
    public Material prisonMat;

    [HideInInspector] public Rigidbody Rb;
    [HideInInspector] public PlayerInputs Input;
    [HideInInspector] public PhotonView photonView;
    [HideInInspector] public PlayerInventory Inventory;
    [HideInInspector] public CheckForGrounded GroundCheck;
    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public Camera cam;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInputs>();
        photonView = GetComponent<PhotonView>();
        Inventory = GetComponent<PlayerInventory>();
        GroundCheck = GetComponent<CheckForGrounded>();

        if (photonView.IsMine)
        {
            for (int i = 0; i < disableRenderForLocalPlayer.Count; i++)
                disableRenderForLocalPlayer[i].layer = 11;

            uiManager = Instantiate(uiPrefab, Vector3.zero, Quaternion.identity).GetComponent<UIManager>();
        }
        else
            Rb.isKinematic = true;

        foreach (var c in GetComponents<PlayerComponent>())
        {
            c.Init();
            // components.Add(c);

            if (c is CameraRotation)
                cam = c.GetComponent<CameraRotation>().GetCamera();
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => TeamManager.Instance.OnTeamRecieved += SetPlayerColor;
    private void OnDisable() => TeamManager.Instance.OnTeamRecieved -= SetPlayerColor;

    private void SetPlayerColor(int playerId)
    {
        if (photonView.OwnerActorNr == playerId)
            playerMesh.material = (TeamManager.Instance.LocalPlayerTeam == Teams.Guard) ? playerMesh.material = guardMat : playerMesh.material = prisonMat;
    }
}
