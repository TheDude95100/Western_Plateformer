using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public HUD HUD;
    public MenuPause MenuPause;
    private void Awake()
    {
        Instance = this;
    }
}
