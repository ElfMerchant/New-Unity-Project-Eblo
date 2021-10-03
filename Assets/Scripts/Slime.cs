using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Entity
{
    private Animator anim;
    private Collider2D col;


    private void Start()
    {
        lives = 3;
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lives > 0 && collision.gameObject == Eblo.Instance.gameObject)
        {
            Eblo.Instance.GetDamage();
            lives--;
            Debug.Log("Slime lives counter:" + lives);

        }

    if (lives < 1)

            Die();     
    }

    public override void Die()
    {
        col.isTrigger = true;
        anim.SetTrigger("death");
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
