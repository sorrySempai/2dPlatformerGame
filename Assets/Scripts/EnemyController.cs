using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	private Rigidbody2D rb;
	public float speed = 7f;
	float direction = -1f;

	public float distancePatrol;
	private bool patrol = true;

	private float minDistance;
	private float maxDistance;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		minDistance = transform.position.x - distancePatrol;
		maxDistance = transform.position.x + distancePatrol;
	}
	void Update()
	{
		if (patrol)
        {
			Patrol();
        }
		if (transform.position.y < -7)
        {
			Destroy(this.gameObject);
        }
	}

	private void Patrol()
    {
		transform.Translate(transform.right * speed * Time.deltaTime);
		if (transform.position.x > maxDistance)
        {
			speed *= -1;
			transform.rotation = Quaternion.Euler(0, 180, 0);
        }
		if (transform.position.x < minDistance)
		{
			speed *= -1;
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

}