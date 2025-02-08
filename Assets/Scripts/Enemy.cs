using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GridManager;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = .05f;

    bool chooseNewPosition;
    private Vector3 nextPosition;
    void Start()
    {
        chooseNewPosition = true;
        nextPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
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
}
