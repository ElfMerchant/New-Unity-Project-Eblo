using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private bool facingRight;
    [SerializeField] private float distance;
    private bool isRecharged;
    private bool isAttacking;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    RaycastHit2D ebloInfo;

    public Transform groundDetection;

    private StatesSkeleton State
    {
        get { return (StatesSkeleton)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    void Start()
    {
        
    }

    private void Awake()
    {
        isAttacking = false;
        isRecharged = true;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

        void Update()
    {
        CheckPlayer();
        if (ebloInfo)
        {
            State = StatesSkeleton.skeleton_attack;
            Attack();
        }
        else
        {
            State = StatesSkeleton.skeleton_idle;
        }
    }

    void CheckPlayer()
    {
        if (facingRight)
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, distance * 1.1f, LayerMask.GetMask("Player"));
        else
            ebloInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, distance * 1.1f, LayerMask.GetMask("Player"));
//        Debug.Log(ebloInfo);
    }

    private void Attack()
    {
        if (isRecharged)
        {
//            Debug.Log("Attack");
            isAttacking = true;
            isRecharged = false;
            Eblo.Instance.GetDamage();

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.45f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }
}

public enum StatesSkeleton
{
    skeleton_idle,
    skeleton_attack,
    skeleton_dead
}