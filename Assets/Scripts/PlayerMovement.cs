using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpforce = 1f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.MovePosition(rb.position + Vector2.right * moveX * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector2.up * 8000);
    }
}
