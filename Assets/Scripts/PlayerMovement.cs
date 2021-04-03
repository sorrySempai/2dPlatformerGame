using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : Unit
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float jumpForce = 1900f;
    [SerializeField] private int lives = 3;
    [SerializeField] private float hurtForce = 10f;

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
    private LivesBar livesBar;

    private bool isGrounded;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public Vector2 moveVector;

    public Transform groundCheck;
    public float spawnX, spawnY;
    private bool isDead;

    void Start()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rb = GetComponent<Rigidbody2D>();
        spawnX = transform.position.x;
        spawnY = transform.position.y;
    }

    void Update()
    {
        Move();
        Jump();
        if (lives <= 0)
        {
            Dead();
        }
    }

    private void Move()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
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
        CheckGround();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Apple")
        {
            Destroy(col.gameObject);
        }
        
        if (col.gameObject.tag == "Enemy")
        {
            if (!isGrounded)
            {
                Destroy(col.gameObject);
            }
            else
            {
                if (col.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
       if (col.gameObject.name == "Death Place")
        {
            Dead();
        }
    }

    public void Dead()
    {
        if (isDead) { return; }
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
