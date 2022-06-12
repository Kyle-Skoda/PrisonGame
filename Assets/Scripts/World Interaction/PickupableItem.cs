using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PickupableItem : MonoBehaviour, IInteractable
{
    public bool IsDroppable = true;
    public bool CanBePickedUp = true;
    public InventoryItem item;

    public PhotonView photonView { get; private set; }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    public void Interact(PlayerInventory inventory)
    {
        if (CanBePickedUp && inventory.player.photonView.IsMine)
            inventory.AddItemToInventory(this);
    }

    [PunRPC]
    public void SetItemHeldState(bool isHolding)
    {
        gameObject.SetActive(isHolding);
    }

    [PunRPC]
    public void Pickup(int playerPickupID, int itemID)
    {
        if (photonView.ViewID != itemID) return;
        PlayerObject player = PhotonView.Find(playerPickupID).GetComponent<PlayerObject>();
        if (player == null) return;
        Transform parent = player.Inventory.ItemLocation();
        transform.parent = parent;
        transform.position = parent.position;
        transform.rotation = parent.rotation;
        rb.isKinematic = true;
        CanBePickedUp = false;
    }

    [PunRPC]
    public void Drop()
    {
        if (!IsDroppable) return;

        rb.isKinematic = false;
        transform.parent = null;
        CanBePickedUp = true;
    }

    public void Interact() { }
}