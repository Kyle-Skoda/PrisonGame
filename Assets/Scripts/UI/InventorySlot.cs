using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Color highlightedColour;
    [SerializeField] private Color defaultColour;

    private Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void SetImage(Sprite newImage)
    {
        itemImage.sprite = newImage;
        if (newImage != null)
            itemImage.enabled = true;
        else
            itemImage.enabled = false;
    }

    public void SetHighlightState(bool highlighted) => slotImage.color = (highlighted) ? highlightedColour : defaultColour;
}
