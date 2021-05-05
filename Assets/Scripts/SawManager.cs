using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawManager : ObstacleManager
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float leftPoint;
    [SerializeField] private float rightPoint;

    bool movingRight = true;
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 3f));
        if (gameObject.tag == "MovingSaw")
        {
            Move();
        }
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
