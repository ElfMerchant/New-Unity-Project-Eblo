using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : Entity
{
    Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lives = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Eblo"))
            {
            rb.isKinematic = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Eblo"))

        {
            Eblo.Instance.Die(); // Либо .GetDamage;
        }
    }

}
