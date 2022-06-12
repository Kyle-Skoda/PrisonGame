using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : PlayerComponent
{
    [SerializeField] private float throwStrength;

    private PickupableItem item;
    private bool shouldThrow;

    public override void Init()
    {
        base.Init();
    }

    private void OnEnable()
    {
        if (!player.photonView.IsMine) return;
        player.Input.OnClick += OnThrow;
        player.Input.OnScroll += OnScroll;
    }
    private void OnDisable()
    {
        if (!player.photonView.IsMine) return;
        player.Input.OnClick -= OnThrow;
        player.Input.OnScroll -= OnScroll;
    }

    private void OnScroll(bool v) => OnCancelThrow();

    private void OnThrow(bool clickStarted)
    {
        if (clickStarted) OnStartThrow();
        else OnEndThrow();
    }

    private void OnCancelThrow() => shouldThrow = false;
    private void OnStartThrow()
    {
        if (player.Inventory.DoesCurrentSlotHaveItem())
        {
            item = player.Inventory.GetCurrentPickupableItem();
            shouldThrow = true;
        }
    }

    private void OnEndThrow()
    {
        if (!shouldThrow) return;
        shouldThrow = false;
        item.gameObject.SetActive(true);
        player.Inventory.DropItemFromInventory(item);
        item.GetComponent<Rigidbody>().velocity = 5 * player.cam.transform.forward;
        item = null;
    }
}

