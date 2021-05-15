using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : Unit
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float speedNormal = 8f;
    [SerializeField] private float jumpForce = 1900f;
    [SerializeField] private int lives = 3;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int apples = 0;
    [SerializeField] private TextMeshProUGUI appleText;
    [SerializeField] private AudioSource appleSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteHurt;
    [SerializeField] private Sprite spriteDead;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 3)
                lives = value;
            livesBar.Refresh();
        }
    }
    public Vector2 moveVector;
    public Transform groundCheck;
    
    private LivesBar livesBar;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D coll;
    

    private bool isGrounded;
    private bool isDead;

    // State Machine
    private enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;

    void Start()
    {
        speed = 0f;
        livesBar = FindObjectOfType<LivesBar>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (state != State.hurt)
        {
            if (sprite.sprite == spriteHurt)
                sprite.sprite = spriteNormal;
            Move();
        }
        Jump();
        if (lives <= 0)
            Dead();
        VelocitySwitch();
        
    }

    private void Move()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && coll.IsTouchingLayers(ground))
        {
            state = State.jumping;
            jumpSound.Play();
            rb.AddForce(Vector2.up * jumpForce);
        }
    }
    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1.3f);
        isGrounded = collider.Length > 1;
    }

    public override void ReceiveDamage(int dmg)
    {
        Lives -= dmg;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        CheckGround();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Collectable")
        {
            appleSound.Play();
            Destroy(col.gameObject);
            apples++;
            appleText.text = apples.ToString();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
       if (col.gameObject.name == "Death Place")
       {
           Dead();
       }

       if (col.gameObject.CompareTag("Enemy"))
       {

           EnemyController ec = col.gameObject.GetComponent<EnemyController>(); 
           if (state == State.falling)
           {
               ec.DeathAction();
               state = State.jumping;
               rb.AddForce(Vector2.up * jumpForce);
            }
           else
           {
                state = State.hurt;
                sprite.sprite = spriteHurt;
                if (col.gameObject.transform.position.x > transform.position.x)
                {
                    // Враг справа от меня --> я отлетаю влево
                    speed = -speedNormal * 1.7f;
                }
                else
                {
                    // Враг слева от меня --> я отлетаю вправо
                    speed = speedNormal * 1.7f;
                }
            }
          
       }
        if (col.gameObject.CompareTag("Boss"))
        {

            BossScript bs = col.gameObject.GetComponent<BossScript>();
            if (state == State.falling)
            {
                bs.GetDamage();
                state = State.jumping;
                rb.AddForce(Vector2.up * jumpForce * 0.9f);
            }
            else
            {
                state = State.hurt;
                sprite.sprite = spriteHurt;
                if (col.gameObject.transform.position.x > transform.position.x)
                {
                    // Враг справа от меня --> я отлетаю влево
                    speed = -speedNormal * 2f;
                }
                else
                {
                    // Враг слева от меня --> я отлетаю вправо
                    speed = speedNormal * 2f;
                }
            }

        }
    }

    public void Dead()
    {
        sprite.sprite = spriteDead;
        if (isDead) { return; }
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnJumpButtonDown()
    {
        if (isGrounded && coll.IsTouchingLayers(ground))
        {
            state = State.jumping;
            jumpSound.Play();
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    public void OnLeftButtonDown()
    {
        if (speed >= 0f)
            speed = -speedNormal;
    }
    public void OnRightButtonDown()
    {
        if (speed <= 0f)
            speed = speedNormal;
    }

    public void OnButtonUp()
    {
        speed = 0f;
    }
    private void VelocitySwitch()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 1f)
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > Mathf.Epsilon && Mathf.Abs(rb.velocity.y) < Mathf.Epsilon)
        {
            state = State.running;
        }
        else
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
            else
            {
                state = State.falling;
            }
        }
    }
}
