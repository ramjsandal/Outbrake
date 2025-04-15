using System;
using UnityEngine;
using static GridManager;

public class Player : MonoBehaviour
{
    private int health = 100;
    public int Health
    {
        get { return health; }
        set { 
            health = value;
            OnHealthChanged(null);
        }
    }

    public event EventHandler<EventArgs> HealthChanged;
    public void OnHealthChanged(EventArgs e)
    {
        if (HealthChanged != null)
        {
            HealthChanged(this, e);
        }
    }


    public int damage = 100;

    private Rigidbody2D rb;

    // Lowkey arbitrarily set. Might need to increase top speed if you want it to be "faster"
    private float topSpeed = 1.25f;
    public float TopSpeed
    {
        get { return topSpeed; }
        set { topSpeed = value; }
    }

    // constanct which we use to control car rotation
    private float rotationalConstant = 2.25f;
    public float RotationalConstant
    {
        get { return rotationalConstant; }
        set { rotationalConstant = value; }
    }

    // used for damage reduction
    public float damageReduction = 1.00f;
    public float DamageReduction
    {
        get { return damageReduction; }
        set { damageReduction = value; }
    }

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

        float topSpeed = this.topSpeed;
        if (gridManager.OnRoad(transform.position))
        {
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

        // Going forwards?
        Vector2 vec = new Vector2(rb.transform.up.x, rb.transform.up.y);
        bool movingForwards = (rb.velocity.normalized - vec).magnitude < 1;
        //Debug.Log((rb.velocity.normalized - vec));

        bool canRotate = rb.velocity.magnitude > 0.5;

        if (canRotate)
        {
            // Update rotation
            // Negative bc idk it just didn't work.
            // 2.5 to make it rotate faster
            float horizontalInput = Input.GetAxis("Horizontal");
            float updatedAngle = (rb.rotation + ((movingForwards ? -1 : 1) * RotationalConstant * horizontalInput));
            rb.MoveRotation(updatedAngle);
        }

        Camera.main.transform.position = this.transform.position + new Vector3(0, 0, -10);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    //private void Update()
    //{

    //}

    public void TakeDamage(int damage)
    {
        Health -= (int)(damage * DamageReduction);
        Debug.Log("Health: " + Health);
        if (Health <= 0)
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
