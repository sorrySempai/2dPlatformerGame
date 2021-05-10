using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject player;
    [SerializeField] private int dmg = 1;
    [SerializeField] private int bossLives = 8;
    [SerializeField] private float bossForce = 45f;
    [SerializeField] private Sprite angryShark;
    [SerializeField] private Sprite normalShark;
    [SerializeField] private GameObject enemy;

    private AudioSource deathSound;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float spawnRate = 1.7f;
    private float randX;
    private float nextSpawn = 0f;

    Vector2 whereToSpawn;


    public Slider healthBar;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        deathSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
        healthBar.value = bossLives;
        if (bossLives == 7 || bossLives == 5 || bossLives == 2)
        {
            Acceleration();
        }
        else
        {
            NotAcceleration();
        }
        if (Time.time > nextSpawn)
            SpawnMobs();
        if (bossLives == 0)
            SceneManager.LoadScene("EndScene");
    }

    private void Move()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            sr.flipX = true;
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            sr.flipX = false;
        }

    }

    private void Acceleration() //—пособность босса - ускорение - на 3 секунды вырастает скорость
    {
        sr.sprite = angryShark;
        speed = 19f;
    }

    private void NotAcceleration() 
    {
        sr.sprite = normalShark;
        speed = 10f;
    }

    private void SpawnMobs() // —пособность босса - создание мобов
    {
        nextSpawn = Time.time + spawnRate;
        randX = Random.Range(10f, 45f);
        whereToSpawn = new Vector2(randX, 3f);
        Instantiate(enemy, whereToSpawn, Quaternion.identity);
    }


    public void GetDamage()
    {
        if (sr.flipX)
        {
            rb.velocity = new Vector2(-bossForce, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(bossForce, rb.velocity.y);
        }
        bossLives--;
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
