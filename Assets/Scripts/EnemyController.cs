using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private float leftPoint;
    [SerializeField] private float rightPoint;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int dmg = 1;

    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sr;
    private AudioSource deathSound;

    private bool facingLeft= true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }


    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftPoint)
            {

                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightPoint)
            {

                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
       
    }

    
    public void DeathAction()
    {
        GetComponent<Collider2D>().enabled = false;
        sr.flipY = true;
        rb.velocity = new Vector2(Random.Range(-10, 10), Random.Range(-10, -5));
        Invoke("Death", 2f);
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Unit unit = col.GetComponent<Unit>();

        if (unit && unit is PlayerMovement)
        {
            unit.ReceiveDamage(dmg);
            deathSound.Play();
        }
    }

}
