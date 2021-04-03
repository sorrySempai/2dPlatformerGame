using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private List<Transform> points;
	[SerializeField] private float speed;

	private int currentIndex;
	private Vector2 currentPoint;
	private bool walking;
	private bool isDead;

    private void Start()
    {
		currentPoint = points[0].position;
		walking = true;

		ChooseDirection();

	}

    private void Update()
    {
        if (isDead) { return; }

		Walk();
    }

	private void ChooseNextPoint()
    {
		if (currentIndex + 1 < points.Count)
        {
			currentIndex += 1;
        }
        else
        {
			currentIndex = 0;
        }

		currentPoint = points[currentIndex].position;
		ChooseDirection();

	}

	private void ChooseDirection()
    {
		GetComponent<SpriteRenderer>().flipX = currentPoint.x < transform.position.x;
    }

	private void Walk()
    {
		if (walking)
        {
			float step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, currentPoint, step);

			if (Vector3.Distance(transform.position, currentPoint) < 0.3f)
            {
				ChooseNextPoint();
            }
        }
    }
}