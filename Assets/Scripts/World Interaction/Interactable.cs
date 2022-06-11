using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public List<Teams> InteractbleFrom = new List<Teams>();
    private PhotonView photonView;
    public bool IsPickedUp = false;

    protected virtual void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public virtual void Pickup(int pickupPlayerId)
    {
        //Dont allow items that are already picked up to be picked up
        if (IsPickedUp == true || photonView.OwnerActorNr != 0) return;
        photonView.RPC("SetPickupState", RpcTarget.All, true);
        photonView.TransferOwnership(pickupPlayerId);
    }

    public virtual void Drop()
    {
        //Only allow player who picked up interactable to drop the item
        if (photonView.IsMine == false) return;
        photonView.RPC("SetPickupState", RpcTarget.All, false);
        photonView.TransferOwnership(0);
    }

    [PunRPC]
    protected void SetPickupState(bool pickedUp)
    {
        IsPickedUp = pickedUp;
    }
}
