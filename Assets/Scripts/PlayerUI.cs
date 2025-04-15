using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image healthbar;

    private Player player;
    private int playerMaxHealth;
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerMaxHealth = player.Health;
        player.HealthChanged += UpdatePlayerUI;
    }


    private void UpdatePlayerUI(object sender, EventArgs e)
    {
        int playerHp = player.Health;
        healthbar.fillAmount = (float) playerHp / (float) playerMaxHealth;
    }

    
}
