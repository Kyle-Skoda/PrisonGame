using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InventorySlotManager InvenentoryModule;

    private UIScreen currentActiveScreen;

    private void Awake()
    {
        InvenentoryModule.SetScreenActive(true);
    }

    public void ChangeScreen(Screens newScreen)
    {
        if (currentActiveScreen.Screen == newScreen) return;

        switch (newScreen)
        {
            case Screens.Game:
                InvenentoryModule.SetScreenActive(true);
                break;
            default:
                Debug.LogWarning($"Screen {newScreen} does not exist");
                return;
        }
        currentActiveScreen.SetScreenActive(false);
    }
}
