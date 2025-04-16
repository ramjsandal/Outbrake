using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    private static ZombiePool _instance;
    public static ZombiePool Instance { get { return _instance; } }

    [SerializeField] private int initialZombiesToPool = 500;

    [SerializeField] private GameObject zombiePrefab;

    [SerializeField] private GameObject policeZombiePrefab;

    [SerializeField] private Transform zombieParent;

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

            for (int i = 0; i < initialZombiesToPool; i++)
            {
                GameObject next = Instantiate(PickZombiePrefab());
                next.transform.SetParent(zombieParent);
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
                next = Instantiate(PickZombiePrefab());
                next.SetActive(false);
            }
            next.SetActive(true);
            numActive++;
            return next;

        } else
        {
            // if we have free money, get one
            GameObject spawn = pool.First(zombie => !zombie.activeSelf);
            spawn.SetActive(true);
            numActive++;
            return spawn;
        }
    }

    public void ReturnToPool(GameObject zombie)
    {
        zombie.SetActive(false);
        numActive--;
    }

    private GameObject PickZombiePrefab()
    {
        int r = Random.Range(0, 100);

        if (r < 10)
        {
            return policeZombiePrefab;
        }
        else
        {
            return zombiePrefab;
        }

    }

}
