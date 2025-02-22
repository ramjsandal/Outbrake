using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Barrel : MonoBehaviour
{
    static Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<Player>();
        }
    }

    // If we collide with the player, check the player's speed
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {

            Vector2 vel = player.GetVelocity();
            // if player going fast enough destroy barrel
            if (vel.magnitude > .0075f)
            {

                Destroy(this.gameObject);
            }
        }
    }
}
