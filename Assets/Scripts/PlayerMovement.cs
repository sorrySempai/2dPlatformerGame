using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : Unit
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float jumpForce = 1900f;
    [SerializeField] private int lives = 3;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int apples = 0;
    [SerializeField] private Text appleText;
    [SerializeField] private float hurtForce = 5f;

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
    public float spawnX, spawnY;
    
    private LivesBar livesBar;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D coll;
    private AudioSource jumpSound;

    private bool isGrounded;
    private bool isDead;

    // State Machine
    private enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;

    void Start()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        jumpSound = GetComponent<AudioSource>();
        spawnX = transform.position.x;
        spawnY = transform.position.y;
    }

    void Update()
    {
        if (state != State.hurt)
        {
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
            rb.AddForce(Vector2.up * jumpForce);
            jumpSound.Play();
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
        CheckGround();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Collectable")
        {
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
               ec.Death();
               state = State.jumping;
               rb.AddForce(Vector2.up * jumpForce);
            }
           else
           {
                state = State.hurt;
                if (col.gameObject.transform.position.x > transform.position.x)
                {
                    // Враг справа от меня --> я отлетаю влево
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    // Враг слева от меня --> я отлетаю вправо
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
          
       }
    }

    public void Dead()
    {
        if (isDead) { return; }
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
