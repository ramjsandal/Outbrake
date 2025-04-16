using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image healthbar;
    public TMP_Text moneyText;

    private Player player;
    private int playerMaxHealth;
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerMaxHealth = player.Health;
        player.HealthChanged += UpdatePlayerUI;
        MoneyPool.Instance.MoneyChanged += UpdateMoneyUI;
    }


    private void UpdatePlayerUI(object sender, EventArgs e)
    {
        int playerHp = player.Health;
        healthbar.fillAmount = (float) playerHp / (float) playerMaxHealth;
    }

    private void UpdateMoneyUI(object sender, EventArgs e)
    {
        moneyText.text = MoneyPool.Instance.PlayerMoney.ToString();
    }
}
