using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Entity
{
    public float speed;
    public float distance;

    private bool movingRight = true;
    private bool wallInfo;

    public Transform groundDetection;

    private Animator anim;

    private void Start()
    {
        lives = 5;
    }

    // Update is called once per frame
    void Update(){

        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
      

        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }
        }

        if (movingRight)
            groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distance);
        else
            groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distance);


        if (groundInfo.collider)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }
        }
    }
       
        

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Eblo.Instance.gameObject)
        {
            Eblo.Instance.GetDamage();
        }
    }

}