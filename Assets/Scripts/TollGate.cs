using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TollGate : MonoBehaviour
{
    [SerializeField]
    int price;

    [SerializeField]
    TMP_Text priceText;

    private void Start()
    {
        priceText.text = price.ToString();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.transform.CompareTag("Player") && MoneyPool.Instance.PlayerMoney >= price)
        {
            MoneyPool.Instance.SpendMoney(price);
            Destroy(this.gameObject);
            UpgradeRandom();
        } 
    }

    private void UpgradeRandom()
    {
        UpgradeManager.Upgrade upgrade = (UpgradeManager.Upgrade) Random.Range(0, 4);
        UpgradeManager.Instance.LevelUpUpgrade(upgrade);
    }
}
