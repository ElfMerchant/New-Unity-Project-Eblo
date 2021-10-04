 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingMonster : Entity
{
    private SpriteRenderer sprite;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private bool isRecharged;
    [SerializeField] readonly private Animator anim;

    public StatesFlying State { get; private set; }
    private StatesFlying GetState()
    { return (StatesFlying)anim.GetInteger("State"); }
    private void SetState(StatesFlying value)
    { anim.SetInteger("State", (int)value); }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        lives = 2;
        isRecharged = true;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.flipX = aiPath.desiredVelocity.x <= 0.01f;
        if (lives < 1)
        {
            State = StatesFlying.flying_die;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Eblo.Instance.gameObject)
        {
            Eblo.Instance.GetDamage();
            isRecharged = false;
            StartCoroutine(AttackCoolDown());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == Eblo.Instance.gameObject)
            StopCoroutine(AttackCoolDown());
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(2f);
        isRecharged = true;
    }
}

public enum StatesFlying
{
    flying_moving,
    flying_die
}