using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [Header("Health")]
    [SerializeField] Image healthBar;

    [Header("Ammo")]
    [SerializeField] Image Ammo;

    string bullet = "Assets/Import/Western_HUD/Bullet_";
    string health = "Western_HUD/Health_";

    public void UpdateHealthBar()
    {
        healthBar.sprite = Resources.Load<Sprite>("Western_HUD/Health_"+player.Health);
        

    }

    public void UpdateBullets()
    {
        Ammo.sprite = Resources.Load<Sprite>("Western_HUD/Bullet_" + player.Ammo);
    }
}
