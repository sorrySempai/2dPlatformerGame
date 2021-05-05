using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    [SerializeField] private float speed = 3f;
    [SerializeField] private float leftPoint;
    [SerializeField] private float rightPoint;

    bool movingRight = true;


    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position.x > rightPoint)
        {
            movingRight = false;
        }
        else if (transform.position.x < leftPoint)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
    }
}
