using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;         

    public Transform groundCheck;
    public LayerMask ground;

    private bool isGround, isJump;

    bool jumpPressed;
    int jumpCount=2;              //可跳跃次数

    Rigidbody2D rb;
    Collider2D coll;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
            
        }
    }

    private void FixedUpdate( )
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        GroundMovement();
        SwitchAnim();
        jump();
    }

    void GroundMovement()                       //奔跑
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");       
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)//进行转向操作。
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);               //人物转向
        }       
    }

    void jump()                     //跳跃
    {
        Debug.Log("jump");
        if (isGround)           //站在地面上
        {
            jumpCount = 2;
            isJump = false;         //站在地面上，就停止播放跳跃动画。
        }
        if(jumpPressed && isGround)         //站在地面上，按下了跳跃键
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;            //可跳跃次数减一
            jumpPressed = false;                //确保跳跃执行了一次
        }else if (jumpPressed && jumpCount>0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;            //可跳跃次数减一
            jumpPressed = false;                //确保跳跃执行了一次
        }
    }

    void SwitchAnim( )
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));               //播放奔跑动画

        if (isGround)
        {
            animator.SetBool("Falling", false);         //落地
        }else if (!isGround && rb.velocity.y > 0)           //向上跳跃
        {
            animator.SetBool("Jumping", true);
        }else if (!isGround && rb.velocity.y < 0)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
    }
}
