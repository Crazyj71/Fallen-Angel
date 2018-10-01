using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : MonoBehaviour {

    public float runForce;
    public float maxRunSpeed;
    private Rigidbody2D rb2d;
    float movementDirection = 1.0f;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();

    }



    void Movement()
    {
        float currentspeed = rb2d.velocity.x;
        if (currentspeed > 0)
        {
            if (currentspeed < maxRunSpeed)
            {
                rb2d.AddForce(new Vector2(movementDirection * runForce, 0.0f));
            }
        }
        else
        {
            if (currentspeed > maxRunSpeed*-1)
            {
                rb2d.AddForce(new Vector2(movementDirection * runForce, 0.0f));
            }
        }

        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            movementDirection *= -1.0f;
        }
    }
    void FixedUpdate()
    {
        Movement();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
