using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static GridManager;

public class Player : MonoBehaviour
{
    public int health = 100;

    public int damage = 100;

    private Rigidbody2D rb;
    // Lowkey arbitrarily set. Might need to increase top speed if you want it to be "faster"
    private float topSpeedBaseline = 1.25f;


    // Increase inertia to be closer to 1 to take longer to slow down
    float inertia = 0.99f;
    float acceleration = 3.0f;

    private GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gridManager = GridManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

        // Moving towards the direction being faced
        float verticalInput = Input.GetAxis("Vertical");
        float verticalForce = verticalInput * acceleration;

        float topSpeed = topSpeedBaseline;
        if (gridManager.OnRoad(transform.position)) {
            verticalForce *= 2;
            topSpeed += 2f;
        }

        // Custom inertia
        rb.velocity = rb.velocity * inertia;

        // Update velocity
        Vector2 forceInput = new Vector2(0, verticalForce);
        rb.AddRelativeForce(forceInput);

        // Cap max speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, topSpeed);

        // Update rotation
        // Negative bc idk it just didn't work.
        // 2.5 to make it rotate faster
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.MoveRotation(rb.rotation + (-2.25f * horizontalInput));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public void Die()
    {
        SceneLoader.LoadScene(0);
    }

}
