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

    [SerializeField]
    protected int kbModifier;

    private Player player;
    private Rigidbody2D rb;
    private int currentHealth;

    bool chooseNewPosition;
    private Vector3 nextPosition;
    protected void Start()
    {
        chooseNewPosition = true;
        nextPosition = transform.position;
        player = FindObjectOfType<Player>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    bool stunned = false;
    void Move()
    {
        if (chooseNewPosition)
        {
            StartCoroutine(ChoosePosition());
        }

        if (!stunned)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);
            Vector2Int posn = GridManager.Instance.GetCellPosition(nextPosition);
            List<Vector2Int> list = new List<Vector2Int>() { posn };
            GridManager.Instance.TintTiles(list, Color.red);
        }
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

    IEnumerator Knockback()
    {
        stunned = true;
        rb.isKinematic = false;
        rb.freezeRotation = false;
        rb.AddForce((transform.position - nextPosition).normalized * kbModifier);
        yield return new WaitForSeconds(2);
        rb.isKinematic = true;
        rb.SetRotation(0);
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        stunned = false;
    } 

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col != null && col.gameObject.CompareTag("Player"))
        {
            // Damage player
            player.TakeDamage(damage);

            // Take Damage
            this.TakeDamage(player.damage);

            StartCoroutine(Knockback());
            
        }

    }

    void Die()
    {
        MoneyPool moneyPool = MoneyPool.Instance;

        float xOffset = 0;
        float yOffset = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < moneyDrop; i++)
        {
            GameObject money = moneyPool.GetPooledObject();
            xOffset = UnityEngine.Random.Range(0.0f, 3.0f);
            yOffset = UnityEngine.Random.Range(0.0f, 3.0f);
            money.transform.position = new Vector3(pos.x + xOffset, pos.y + yOffset, 0);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
