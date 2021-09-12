using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eblo : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // �������� ������������
    [SerializeField] private int lives = 3; // ���������� ������
    [SerializeField] private float jumpforce = 15f; // ���� ������
    private bool isGrounded = false; // ���� �� ���������� �� �����, ����� �������� ����������� ������������ ������ ( !�������� �� ���� �� ������������ ���������!)

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", ( int)value); }
    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent <Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
     
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

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.jump;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGrounded) State = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();

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
    jump
}