using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GridManager;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<NodeInfo> path = GridManager.Instance.GetPathToPlayer(this.transform.position);
        NodeInfo next = GridManager.Instance.SmoothPath(transform.position, path);
        List<Vector2Int> vector2Ints = new List<Vector2Int>() { next.position };
        GridManager.Instance.TintTiles(vector2Ints, Color.red);
    }
}
