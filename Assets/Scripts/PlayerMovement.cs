using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpForce = 1900f;
    private float moveInput;

    private Rigidbody2D rb;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float spawnX, spawnY;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnX = transform.position.x;
        spawnY = transform.position.y;
    }

    void Update()
    {
        if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Apple")
        {
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Saw" || col.gameObject.name == "Death Place" || col.gameObject.name == "Spikes")
        {
            transform.position = new Vector3(spawnX, spawnY, transform.position.z);
        }
    }

}
