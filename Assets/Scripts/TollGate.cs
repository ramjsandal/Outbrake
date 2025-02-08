using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TollGate : MonoBehaviour
{
    [SerializeField]
    int price;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.transform.CompareTag("Player") && MoneyPool.Instance.playerMoney >= price)
        {
            MoneyPool.Instance.SpendMoney(price);
            Destroy(this.gameObject);
        } 
    }
}
