using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System;

public class MushroomBoss : Entity
{
    [SerializeField] private float speedBoss;
    [SerializeField] private float distanceBoss;

    [SerializeField] private bool isAttackingBoss;
    [SerializeField] private bool isRechargedBoss;
    [SerializeField] readonly private Animator anim;

    private Rigidbody2D rbBoss;
    private bool movingRight;
    private bool wallInfo;

    public Transform groundDetection;

    //    public Fungus.Flowchart flowchart;
    //    private bool inRange;
    //    private bool inBattle;

    RaycastHit2D ebloInfo;

    private void Start()
    {
        lives = 10;
        movingRight = true;
        isAttackingBoss = false;
        isRechargedBoss = true;
        Instance = this;
    }

    public StatesBoss State { get; private set; }
    public static MushroomBoss Instance { get; private set; }

    private StatesBoss GetState()
    { return (StatesBoss)anim.GetInteger("State"); }
    private void SetState(StatesBoss value)
    { anim.SetInteger("State", (int)value); }

    void Update()
    {
        //        Debug.Log(State);
        CheckPlayer();

        if (ebloInfo)
        {
            State = StatesBoss.mushroom_attack;
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
//            StateBoss = States.mushroom_die;
            Die();
        }

    }

    void CheckPlayer()
    {
        if (movingRight)
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distanceBoss * 1.1f, LayerMask.GetMask("Player"));
        else
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distanceBoss * 1.1f, LayerMask.GetMask("Player"));
        //        Debug.Log(ebloInfo);
    }


    void MoveBoss()
    {
        //        Debug.Log("Boss is approaching!");
        State = StatesBoss.mushroom_idle;
        isAttackingBoss = false;
        isRechargedBoss = true;

        transform.Translate(Vector2.right * speedBoss * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceBoss);
        Debug.Log(groundInfo.collider);

        //if (groundInfo.collider == false)
        //{
        //    if (movingRight == true)
        //    {
        //        transform.eulerAngles = new Vector3(0, -180, 0);
        //        movingRight = false;
        //    }
        //    else
        //    {
        //        {
        //            transform.eulerAngles = new Vector3(0, 0, 0);
        //            movingRight = true;
        //        }
        //    }
        //}

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

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
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


}


public enum StatesBoss
{
    mushroom_idle,
    mushroom_attack,
    //    mushroom_die
}