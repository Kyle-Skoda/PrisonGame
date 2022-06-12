using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    public Screens Screen;

    public virtual void SetScreenActive(bool active) => gameObject.SetActive(active);
}

public enum Screens
{
    None,
    Game
}