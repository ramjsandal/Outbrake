using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    ZombiePool pool;

    public void Start()
    {
        pool = ZombiePool.Instance;
        InvokeRepeating("SpawnZombie", 1, 3);
    }

    Vector3 PickEnemySpawnLocation()
    {
        int r = Random.Range(0, 4);
        float other = ((float)Random.Range(0, 100)) / 100f;
        Vector3 spawnLocation;
        switch (r)
        {
            case 0:
                spawnLocation = new Vector3(-.1f, other);
                break;
            case 1:
                spawnLocation = new Vector3(other, -.1f);
                break;
            case 2:
                spawnLocation = new Vector3(1.1f, other);
                break;
            case 3:
                spawnLocation = new Vector3(other, 1.1f);
                break;
            default:
                throw new System.Exception("WHAWKAHDJSJLKDSA");
        }

        return Camera.main.ViewportToWorldPoint(spawnLocation);
    }

    void SpawnZombie()
    {
        GameObject zom = pool.GetPooledObject();
        zom.transform.position = PickEnemySpawnLocation(); 
        if (!GridManager.Instance.OnTraversableTile(zom.transform.position))
        {
            pool.ReturnToPool(zom);
        }
    }
}
