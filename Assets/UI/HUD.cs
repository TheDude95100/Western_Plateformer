using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] PlayerController player;

    PlayerManager playerManager;

    [Header("Health")]
    [SerializeField] Image healthBar;

    [Header("Ammo")]
    [SerializeField] Image Ammo;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI Score;

    string bullet = "Assets/Import/Western_HUD/Bullet_";
    string health = "Western_HUD/Health_";

    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    private void Update()
    {
       UpdateScore();
    }

    public void UpdateHealthBar()
    {
        healthBar.sprite = Resources.Load<Sprite>("Western_HUD/Health_" + player.Health);
    }

    public void UpdateBullets()
    {
        Ammo.sprite = Resources.Load<Sprite>("Western_HUD/Bullet_" + player.Ammo);
    }

    public void UpdateScore()
    {
        Score.text = $"Score:{playerManager.Score}";
    }


}
