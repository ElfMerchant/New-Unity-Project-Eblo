using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Entity
{ 
    [SerializeField] private int lives = 3;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Eblo.Instance.gameObject)
        {
            Eblo.Instance.GetDamage();
            lives--;
            Debug.Log("Slime lives counter:" + lives);

        }

    if (lives < 1)
        Die();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
