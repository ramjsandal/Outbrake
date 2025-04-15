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
    private GridManager gridManager;

    bool chooseNewPosition;
    private Vector3 nextPosition;
    protected void Start()
    {
        chooseNewPosition = true;
        nextPosition = transform.position;
        player = FindObjectOfType<Player>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        gridManager = GridManager.Instance;
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
            Vector2Int posn = gridManager.GetCellPosition(nextPosition);
        }
    }

    IEnumerator ChoosePosition()
    {
        chooseNewPosition = false;
        List<NodeInfo> path = gridManager.GetPathToPlayer(this.transform.position);
        NodeInfo next = gridManager.SmoothPath(transform.position, path);
        //NodeInfo next = path.Count > 1 ? path[1] : path[0];
        nextPosition = gridManager.GetWorldPosition(next.position);
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
        yield return new WaitForSeconds(1);
        rb.isKinematic = true;
        rb.SetRotation(0);
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        stunned = false;
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col != null && col.gameObject.CompareTag("Player"))
        {
            Vector2 vel = player.GetVelocity();
            float speed = vel.magnitude;
            //Debug.Log(speed);

            // if player is going too slow, damage the player
            if (speed < .25f)
            {
                // Damage player
                player.TakeDamage(damage);
            }
            else
            {
                // if were going fast enough, damage the zombie
                // based on how fast the player is going
                this.TakeDamage((int)(player.damage * speed));


            }
            if (this.isActiveAndEnabled)
            {
                StartCoroutine(Knockback());
            }

        }

    }

    public void ResetZombie()
    {
        StopAllCoroutines();
        chooseNewPosition = true;
        nextPosition = transform.position;
        currentHealth = maxHealth;
        rb.isKinematic = true;
        rb.SetRotation(0);
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        stunned = false;
    }

    void Die()
    {
        MoneyPool moneyPool = MoneyPool.Instance;

        float xOffset = 0;
        float yOffset = 0;
        Vector3 pos = transform.position;
        float zRotation = 0;
        for (int i = 0; i < moneyDrop; i++)
        {
            GameObject money = moneyPool.GetPooledObject();
            xOffset = UnityEngine.Random.Range(-1.0f, 1.0f);
            yOffset = UnityEngine.Random.Range(-1.0f, 1.0f);
            zRotation = UnityEngine.Random.Range(-180.0f, 180.0f);
            money.transform.position = new Vector3(pos.x + xOffset, pos.y + yOffset, 0);
            money.transform.Rotate(new Vector3(0, 0, zRotation));
        }

        ZombiePool.Instance.ReturnToPool(this.gameObject);
        ResetZombie();
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
