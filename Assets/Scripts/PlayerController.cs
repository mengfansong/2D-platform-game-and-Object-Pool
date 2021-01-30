using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;         

    public Transform groundCheck;
    public LayerMask ground;

    [SerializeField]
    private bool isGround, isJump,isDashing;

    bool jumpPressed;
    int jumpCount=2;              //可跳跃次数

    Rigidbody2D rb;
    Collider2D coll;
    Animator animator;

    public GameObject joyStick;
    [Header("冲刺技能UI组件")]
    public Image dashImage;


    [Header("冲刺技能的参数")]
    public float dashTime;          //冲刺技能持续时间
    private float dashTimeLeft;         //冲刺技能还剩多长时间完成
    public float dashSpeed;         //冲刺时的速度
    private float lastDash=-10f;             //上次使用冲刺的时刻，用以记录判断冲刺技能的CD  初始值为-10，确保游戏一开始就可以冲刺。
    public float dashCoolDown;      //冲刺技能的冷却时长




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

        if (Input.GetKeyDown(KeyCode.K))
        {
            DoDash();
        }

        //技能冷却
        dashImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;

    }

    public void DoDash()                        //按下冲刺键
    {
        //判断是否可以冲刺的条件
        if (Time.time >= lastDash + dashCoolDown)           //冲刺技能冷却完毕
        {
            //可以冲刺
            ReadyToDash();
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if (rb.velocity.y > 0 && !isGround)          //处于跳跃过程中按下了冲刺技能
                {
                    rb.gravityScale = 0;        //冲刺时暂停重力的作用
                }
                
                rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);           //给予冲刺速度
                

                
                dashTimeLeft -= Time.deltaTime;     //剩余时长更新

                ObjectPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                rb.gravityScale = 3;        //冲刺结束恢复重力作用
                //if (!isGround)              //冲刺结束后仍处于空中
                //{
                //    rb.velocity = new Vector2(dashSpeed * transform.localScale.x, jumpForce);
                //}
            }
        }
    }

    public void OnButtonJumpDown()          //按下跳跃键
    {
        if (jumpCount > 0)
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate( )
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        animator.SetBool("IsGround", isGround);
        Dash();
        if (isDashing) { return; }          //冲刺中不允许进行其他操作。
        GroundMovement();
        SwitchAnim();
        Jump();
        
    }

    void GroundMovement()                       //奔跑
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if (joyStick.GetComponent<FixedJoystick>().horizontal != 0)
        {
            horizontalMove = joyStick.GetComponent<FixedJoystick>().horizontal;
        }       
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)//进行转向操作。
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);               //人物转向
        }       
    }

    void Jump()                     //跳跃
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

    private void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;            //冲刺时间计时

        lastDash = Time.time;

        dashImage.fillAmount = 1.0f;        //技能图标变成阴影
    }
}
