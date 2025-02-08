using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridManager;

public class TankControls : MonoBehaviour
{
    // 
    private Rigidbody2D rb; 
    private float topSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Inputs for direction
        float horizontalInput = Input.GetAxis("Horizontal");  
        float verticalInput = Input.GetAxis("Vertical");  

        // If no input, if speed not zero, add force in negative direction
        float horizontalForce = 0;
        float verticalForce = 0;

        if (horizontalInput == 0) {
            if (rb.velocity.x != 0) {
                horizontalForce = -0.25f * rb.velocity.x;
            }
        } else {
            horizontalForce = horizontalInput * 0.75f;    
            if (horizontalInput * rb.velocity.x < 0) {
                horizontalForce *= 2.5f;
            }  
        }

        if (verticalInput == 0) {
            if (rb.velocity.y != 0) {
                verticalForce = -0.25f * rb.velocity.y;
            }
        } else {
            verticalForce = verticalInput * 0.75f;
            if (verticalInput * rb.velocity.y < 0) {
                verticalForce *= 2.5f;
            }
        }

        Vector2 forceInput = new Vector2(horizontalForce, verticalForce);
        rb.AddForce(forceInput);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, topSpeed);
    }
}
