using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : PlayerComponent
{
    [SerializeField] private float maxDistance = 3;
    [SerializeField] private LayerMask interactableLayers;

    private void OnEnable() => player.Input.OnInteract += Interact;
    private void OnDisable() => player.Input.OnInteract -= Interact;

    private void Interact()
    {
        if (Physics.BoxCast(player.cam.transform.position, Vector3.one * 0.25f, player.cam.transform.forward, out RaycastHit hit, Quaternion.identity, maxDistance, interactableLayers))
        {
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable is PickupableItem)
                    interactable.Interact(player.Inventory);
            }
        }
    }
}
