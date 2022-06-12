using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager : UIScreen
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotArea;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private int currentSlot = 0;

    public void Init(int inventorySlots)
    {
        for (int i = 0; i < inventorySlots; i++)
            slots.Add(Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, slotArea).GetComponent<InventorySlot>());
        ChangeSlot(0);
    }

    public void ChangeSlot(int newSlot)
    {
        if (newSlot > slots.Count - 1) return;

        slots[currentSlot].SetHighlightState(false);
        slots[newSlot].SetHighlightState(true);
        currentSlot = newSlot;
    }

    public void SetSlotItem(int slot, Sprite image)
    {
        slots[slot].SetImage(image);
    }
}
