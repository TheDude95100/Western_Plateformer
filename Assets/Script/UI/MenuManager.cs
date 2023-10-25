using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public HUD HUD;
    public MenuPause MenuPause;
    public MenuMort MenuMort;
    public MenuWin MenuWin;

    [SerializeField] PlayerManager playerManager;
    private void Awake()
    {
        Instance = this;
        playerManager = PlayerManager.instance;
    }

}
