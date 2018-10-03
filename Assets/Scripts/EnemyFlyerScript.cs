using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyerScript : MonoBehaviour {

    public GameObject Player;
    public float runForce;
    public float attackForce;
    public float maxRunSpeed;
    public float maxMoveTime;
    private Rigidbody2D rb2d;
    private float movementDirection = 1.0f;
    private float moveTime;
    private bool playerSpotted;

    // Use this for initialization
    void Start()
    {
        playerSpotted = false;
        rb2d = GetComponent<Rigidbody2D>();
        moveTime = 0.0f;
    }

    void Movement()
    {

        moveTime += Time.deltaTime;

        if (moveTime > maxMoveTime) {
            moveTime = 0.0f;
            movementDirection *= -1.0f;
            rb2d.velocity = new Vector2(0.0f,0.0f);
        }

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
            if (currentspeed > maxRunSpeed * -1)
            {
                rb2d.AddForce(new Vector2(movementDirection * runForce, 0.0f));
            }
        }


    }

    void MoveTowardsPlayer()
    {
        Vector2 attackDirection = (Player.transform.position-this.transform.position);

        rb2d.AddForce(attackDirection * attackForce);
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           playerSpotted = true;
        }
    }

     void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerSpotted = false;
        }
    }

    void FixedUpdate()
    {
        if (playerSpotted == false)
        {
            Movement();
        }
        else if (playerSpotted == true)
        {
            MoveTowardsPlayer();
        }

    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;

    }
}
