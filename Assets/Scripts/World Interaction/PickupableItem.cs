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
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        col = GetComponent<Collider>();
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
        photonView.TransferOwnership(player.photonView.OwnerActorNr);
        col.isTrigger = true;
    }

    [PunRPC]
    public void Drop()
    {
        if (!IsDroppable) return;

        rb.isKinematic = false;
        transform.parent = null;
        CanBePickedUp = true;
        col.isTrigger = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            photonView.TransferOwnership(other.gameObject.GetComponent<PhotonView>().OwnerActorNr);
    }

    public void Interact() { }
}