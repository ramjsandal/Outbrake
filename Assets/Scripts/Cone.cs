using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField]
    int kbModifier;

    Rigidbody2D rb;
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
    }
    IEnumerator Knockback()
    {
        rb.isKinematic = false;
        rb.freezeRotation = false;
        rb.AddForce((transform.position - GridManager.Instance.GetPlayerPosition()).normalized * kbModifier);
        yield return new WaitForSeconds(1);
        rb.isKinematic = true;
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(Knockback());
    }


}
