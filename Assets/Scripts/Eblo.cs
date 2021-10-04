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
    [SerializeField] private GameObject losePanel;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource missAttackSound;
    [SerializeField] private AudioSource attackmobSound;
    [SerializeField] private AudioSource deathSound;

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
        losePanel.SetActive(false); //Сам написал, чел в гайде просто не указал эту строчку

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
        rb.velocity = Vector2.up * jumpforce;
        jumpSound.Play();
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

        if (colliders.Length == 0)
            missAttackSound.Play();
        else
            attackmobSound.Play();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            StartCoroutine(EnemyOnAttack(colliders[i]));
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }


    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        enemyColor.color = new Color(1f, 0.4375f, 0.4375f);
        yield return new WaitForSeconds(0.2f);
        enemyColor.color = new Color(1, 1, 1);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1f);
        isGrounded = collider.Length > 1;

        if (!isGrounded && health > 0) State = States.jump;
    }



    public override void GetDamage()
    {
        lives -= 1;
        damageSound.Play();
        Debug.Log("Eblo lives counter:" + lives);
        if (lives < 1) // АЛЯРМ! ВОЗМОЖНО ЭТО НЕ СЮДА, НО ЭТО РАБОТАЕТ
            Die(); // ТАКИЕ ДЕЛА
        
    }

    public override void Die()
    {
        State = States.death;
        Invoke("SetLosePanel", 0.3f);// 0.3f отвечает за задержку перед смертью
        deathSound.Play();
    }

    private void SetLosePanel()//Нужно это дело подшаматить для корректной работы с вызовом Меню и рестартом уровня через смерть.
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Flowchart.ExecuteBlock("Trigger");
    //}

    // Update is called once per frame
    private void Update()
    {

            if (isGrounded && !isAttacking && health > 0) State = States.idle; 

            if (!isAttacking && Input.GetButton("Horizontal"))
                Run();
            if (!isAttacking && isGrounded && health > 0 && Input.GetButtonDown("Jump"))
                Jump();
            if (Input.GetButtonDown("Fire1"))
                Attack();
            if (health < 1) State = States.death; // Написал сам, чтобы при смерти проигрывалась анимация смерти
        



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
    attack,
    death
}