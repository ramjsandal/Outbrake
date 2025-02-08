using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyPool : MonoBehaviour
{
    private static MoneyPool _instance;
    public static MoneyPool Instance { get { return _instance; } }

    public int initialMoneyToPool = 500;

    public GameObject moneyPrefab;

    public int playerMoney;

    private HashSet<GameObject> pool;

    private int numActive;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

            pool = new HashSet<GameObject>();
            numActive = 0;
            playerMoney = 0;

            for (int i = 0; i < initialMoneyToPool; i++)
            {
                GameObject next = Instantiate(moneyPrefab);
                next.SetActive(false);
                pool.Add(next);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        // if all of our money is in use
        if (numActive >= pool.Count)
        {
            GameObject next = null;
            for (int i = 0; i < 10; i++)
            {
                next = Instantiate(moneyPrefab);
                next.SetActive(false);
            }
            next.SetActive(true);
            numActive++;
            return next;

        } else
        {
            // if we have free money, get one
            GameObject spawn = pool.First(dollar => !dollar.activeSelf);
            spawn.SetActive(true);
            numActive++;
            return spawn;
        }
    }

    public void ReturnToPool(GameObject money)
    {
        money.SetActive(false);
        playerMoney++;
        numActive--;
    }

}
