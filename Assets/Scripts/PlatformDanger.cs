using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDanger : Entity
{
    Rigidbody2D rb;
    public bool isGrounded;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Eblo"))
            {
            rb.isKinematic = false;
        }
    }


}
