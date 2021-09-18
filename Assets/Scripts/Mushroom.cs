using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Entity
{
    private float speed = 3f;
    private Vector3 dir;
    private SpriteRenderer sprite;


    private void Start()
    {
        dir = transform.right;
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);
        if (colliders.Length > 0) dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Eblo.Instance.gameObject)
        {
            Eblo.Instance.GetDamage();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }   
}
