using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridManager;

public class TankControls : MonoBehaviour
{
    enum Direction
    {

        Left, UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft
    };
    private Rigidbody2D rb; 
    // Lowkey arbitrarily set. Might need to increase top speed if you want it to be "faster"
    private float topSpeed = 8f;

    private SpriteRenderer spr;

    public List<Sprite> sprites = new List<Sprite>();
    // Increase inertia to be closer to 1 to take longer to slow down
    // 
    float inertia = 0.99f;
    float acceleration = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moving towards the direction being faced
        float verticalInput = Input.GetAxis("Vertical");  
        float verticalForce = verticalInput * acceleration;

        // Custom inertia
        rb.velocity = rb.velocity * inertia;

        // Update velocity
        Vector2 forceInput = new Vector2(0, verticalForce);
        rb.AddRelativeForce(forceInput);

        // Cap max speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, topSpeed);

        //spr.sprite = sprites[(int)GetSpriteDirection()];
        //Debug.Log(GetSpriteDirection());


        // Update rotation
        // Negative bc idk it just didn't work.
        // 2.5 to make it rotate faster
        float horizontalInput = Input.GetAxis("Horizontal");  
        rb.MoveRotation(rb.rotation + (-2.25f * horizontalInput));
    }

    Direction GetSpriteDirection()
    {
        Vector2 direction = rb.velocity.normalized;

        if (direction.x >= -.5 && direction.x <= .5)
        {
            if (direction.y > 0)
            {
                return Direction.Up;
            } else
            {
                return Direction.Down;
            }
        }

        if (direction.y >= -.5 && direction.y <= .5)
        {
            if (direction.x > 0)
            {
                return Direction.Right;
            } else
            {
                return Direction.Left;
            }
        }

        if (direction.x > 0)
        {
            if (direction.y < 0)
            {
                return Direction.DownRight;
            } else
            {
                return Direction.UpRight;
            }
        } else
        {
            if (direction.y < 0)
            {
                return Direction.DownLeft;
            } else
            {
                return Direction.UpLeft;
            }
 
        }

    }
}
