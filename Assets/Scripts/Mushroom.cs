using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Entity
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isRecharged;
    [SerializeField] readonly private Animator anim;

    private bool movingRight;
    private bool wallInfo;

    public Transform groundDetection;
    RaycastHit2D ebloInfo;

    private void Start()
    {
        lives = 5;
        movingRight = true;
    }

    public StatesMushroom State { get; private set; }
    public static MushroomBoss Instance { get; private set; }

    private StatesMushroom GetState()
    { return (StatesMushroom)anim.GetInteger("State"); }
    private void SetState(StatesMushroom value)
    { anim.SetInteger("State", (int)value); }

    void Update()
    {
        CheckPlayer();

        if (ebloInfo)
        {
            State = StatesMushroom.mushroom_attack;
            Attack();
        }

        if (isAttacking == false)
        {
            Move();
        }
        if (lives < 1)
        {
            Die();
        }

    }

    void CheckPlayer()
    {
        if (movingRight)
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distance * 1.1f, LayerMask.GetMask("Player"));
        else
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distance * 1.1f, LayerMask.GetMask("Player"));
    }


    void Move()
    {
        State = StatesMushroom.mushroom_idle;
        isAttacking = false;
        isRecharged = true;

        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        Debug.Log(groundInfo.collider);

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

    private void Attack()
    {
        if (isRecharged)
        {
            isAttacking = true;
            isRecharged = false;
            Eblo.Instance.GetDamage();

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(1f);
        isRecharged = true;
    }


}
public enum StatesMushroom
{
    mushroom_idle,
    mushroom_attack,
    //    mushroom_die
}