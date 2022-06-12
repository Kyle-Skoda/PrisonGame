using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInventory : PlayerComponent
{
    public int MaxInventorySlots = 6;

    [SerializeField] private Transform itemHoldLocation;
    [SerializeField] private List<PickupableItem> defaultItems = new List<PickupableItem>();

    private int currentSlot = 0;
    private Dictionary<int, PickupableItem> items = new Dictionary<int, PickupableItem>();

    //TODO: Display held item
    //TODO: Sync items in player inventories

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < MaxInventorySlots; i++)
        {
            if (defaultItems.Count > i)
                items.Add(i, defaultItems[i]);
            else
                items.Add(i, null);
        }

        if (player.photonView.IsMine)
            player.uiManager.InvenentoryModule.Init(MaxInventorySlots);
    }

    private void OnEnable() => player.Input.OnScroll += OnScroll;
    private void OnDisable() => player.Input.OnScroll -= OnScroll;

    private void OnScroll(bool positive)
    {
        items[currentSlot]?.photonView.RPC("SetItemHeldState", RpcTarget.All, false);

        if (!positive)
            currentSlot++;
        else
            currentSlot--;

        if (currentSlot >= MaxInventorySlots)
            currentSlot = 0;
        else if (currentSlot < 0)
            currentSlot = MaxInventorySlots - 1;

        player.uiManager.InvenentoryModule.ChangeSlot(currentSlot);
        items[currentSlot]?.photonView.RPC("SetItemHeldState", RpcTarget.All, true);
    }

    public void AddItemToInventory(PickupableItem newItem)
    {
        int setSlotItem = currentSlot;
        if (items[currentSlot] == null)
            items[currentSlot] = newItem;
        else
            for (int i = 0; i < items.Count; i++)
                if (items[currentSlot] == null)
                {
                    items[i] = newItem;
                    setSlotItem = i;
                }

        items[setSlotItem].photonView.RPC("Pickup", RpcTarget.All, player.photonView.ViewID, items[setSlotItem].photonView.ViewID);
        player.uiManager.InvenentoryModule.SetSlotItem(setSlotItem, newItem.item.inventoryImage);
    }

    public void DropItemFromInventory(PickupableItem newItem)
    {
        if (items[currentSlot] == null)
            return;
        else if (items[currentSlot].IsDroppable)
        {
            items[currentSlot].photonView.RPC("Drop", RpcTarget.All);
            items[currentSlot] = null;
        }
        else return;


        player.uiManager.InvenentoryModule.SetSlotItem(currentSlot, null);
    }

    public bool DoesInventoryHaveSpace() => items.Count < MaxInventorySlots;
    public bool DoesCurrentSlotHaveItem() => items[currentSlot] != null;
    public PickupableItem GetCurrentPickupableItem() => items[currentSlot];
    public Transform ItemLocation() => itemHoldLocation;
}
