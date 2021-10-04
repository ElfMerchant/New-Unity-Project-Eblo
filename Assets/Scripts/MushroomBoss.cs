using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class MushroomBoss : Entity
{

    public float speedBoss;
    public float distanceBoss;

    private Rigidbody2D rbBoss;
    private bool movingRight = true;
    private bool wallInfo;
    private bool inRange;
    private bool isAttackingBoss = false;
    private bool isRechargedBoss = true;

    public Transform groundDetection;

    private Animator anim;

    RaycastHit2D ebloInfo;

    private void Start()
    {
        lives = 8;
        inRange = false;
        isAttackingBoss = false;
        isRechargedBoss = true;
//        Instance = this;
    }

    //public static MushroomBoss Instance { get; set; }
    //private StatesBoss State
    //{
    //    get { return (StatesBoss)anim.GetInteger("State"); }
    //    set { anim.SetInteger("State", (int)value); }
    //}

    void Update()
    {
        if (movingRight)
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distanceBoss*2, LayerMask.GetMask("Player"));
        else
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distanceBoss*2, LayerMask.GetMask("Player"));
//        Debug.Log(ebloInfo);

        if (ebloInfo)
        {
 //           State = StatesBoss.mushroom_attack;
            Attack();
        }

        if (isAttackingBoss == false)
        {
//            Debug.Log("State Run");
//            State = StatesBoss.mushroom_idle;
            MoveBoss();
        }
        if (lives < 1)
        {
//            StateBoss = States.death;
            Die();
        }

    }


    void MoveBoss()
    {
        Debug.Log("Boss is approaching!");
        isAttackingBoss = false;
        isRechargedBoss = true;

        transform.Translate(Vector2.right * speedBoss * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceBoss);
//        Debug.Log(groundInfo.collider);

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


//        Debug.Log(groundInfo.collider);

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

    private void Attack()
    {
        Debug.Log("Boss is attacking!");
        if (isRechargedBoss)
        {
            isAttackingBoss = true;
            isRechargedBoss = false;
            Eblo.Instance.GetDamage();

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        isAttackingBoss = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(1f);
        isRechargedBoss = true;
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject == Eblo.Instance.gameObject)
    //    {
    //        inRange = true;
    //    }
    //}

    //    private void OnCollisionExit(Collision collision)
    //    {
    //        if (collision.gameObject == Eblo.Instance.gameObject)
    //        {
    //            inRange = false;
    //        }
    //    }

}


public enum StatesBoss
{
    mushroom_idle,
    mushroom_attack
}