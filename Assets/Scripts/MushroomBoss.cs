using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class MushroomBoss : Entity
{
    public float speedBoss;
    public float distanceBoss;
    public float jumpBoss;

    System.Random rnd = new System.Random();

    private Rigidbody2D rbBoss;
    private int ifJump;
    private bool movingRight = true;
    private bool wallInfo;

    public Transform groundDetection;

    private Animator anim;

    private void Start()
    {
        lives = 8;
    }

    // Update is called once per frame
    void Update(){

        transform.Translate(Vector2.right * speedBoss * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceBoss);
      

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
            groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distanceBoss);
        else
            groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distanceBoss);


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
        ifJump = rnd.Next(0, 1000);
        if(ifJump == 0)
            rbBoss.AddForce(transform.up * jumpBoss, ForceMode2D.Impulse);
    }
       
        

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Eblo.Instance.gameObject)
        {
            Eblo.Instance.GetDamage();
        }
    }

}