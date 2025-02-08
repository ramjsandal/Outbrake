using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static GridManager;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    [SerializeField]
    protected int maxHealth;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected int moneyDrop;

    bool chooseNewPosition;
    private Vector3 nextPosition;
    protected void Start()
    {
        chooseNewPosition = true;
        nextPosition = transform.position;
        Invoke("Die", 3);
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    void Move()
    {
        if (chooseNewPosition)
        {
            StartCoroutine(ChoosePosition());
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);
        Vector2Int posn = GridManager.Instance.GetCellPosition(nextPosition);
        List<Vector2Int> list = new List<Vector2Int>() { posn };
        GridManager.Instance.TintTiles(list, Color.red);
 
    }

    IEnumerator ChoosePosition()
    {
        chooseNewPosition = false;
        List<NodeInfo> path = GridManager.Instance.GetPathToPlayer(this.transform.position);
        NodeInfo next = GridManager.Instance.SmoothPath(transform.position, path);
        //NodeInfo next = path.Count > 1 ? path[1] : path[0];
        nextPosition = GridManager.Instance.GetWorldPosition(next.position);
        float timeToReachNextPosition = Vector3.Distance(transform.position, nextPosition) / speed;
        yield return new WaitForSeconds(Math.Min(timeToReachNextPosition, 1));
        chooseNewPosition = true;
    }

    void Die()
    {
        MoneyPool moneyPool = MoneyPool.Instance;

        float xOffset = 0;
        float yOffset = 0;
        for (int i = 0; i < moneyDrop; i++)
        {
            GameObject money = moneyPool.GetPooledObject();
            xOffset = UnityEngine.Random.Range(0, 5);
            yOffset = UnityEngine.Random.Range(0, 5);
            money.transform.position = new Vector3(xOffset, yOffset, 0);
        }
        
        Destroy(gameObject);
    }
}
