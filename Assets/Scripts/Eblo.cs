using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;


public class Eblo : Entity
{
    [SerializeField] private float speed = 10f; // скорость передвижения
    [SerializeField] private float jumpforce = 15f; // сила прыжка
    [SerializeField] private int health = 5; // текущее здоровье
    public bool isGrounded = false; // тест на нахождение на земле, чтобы избежать возможности бесконечного прыжка ( !ЗАМЕНИТЬ НА ТЕСТ НА ВЕРТИКАЛЬНОЕ УСКОРЕНИЕ!)

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;
    

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Eblo Instance { get; set; }

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", ( int)value); }
    }

 

    private void Awake()
    {
        lives = 5;
        health = lives;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent <Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
     
    }   

    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
    }

   

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }


    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.jump;
    }



    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log("Eblo lives counter:" + lives);
        if (lives < 1) // АЛЯРМ! ВОЗМОЖНО ЭТО НЕ СЮДА, НО ЭТО РАБОТАЕТ
            Die(); // ТАКИЕ ДЕЛА
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Flowchart.ExecuteBlock("Trigger");
    //}

    // Update is called once per frame
    private void Update()
    {

            if (isGrounded && !isAttacking) State = States.idle;

            if (!isAttacking && Input.GetButton("Horizontal"))
                Run();
            if (!isAttacking && isGrounded && Input.GetButtonDown("Jump"))
                Jump();
            if (Input.GetButtonDown("Fire1"))
                Attack();

        if (health > lives)
            health = lives;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = aliveHeart;
            else
                hearts[i].sprite = deadHeart; //deadHeart не отображается

            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
   
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

} 

public enum States
{ 
    idle,
    run,
    jump,
    attack
}