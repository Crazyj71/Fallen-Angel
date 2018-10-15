using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : MonoBehaviour {

    public float runForce;
    public float maxRunSpeed;
    public float health;
    public float dam;
    public int hitForce;


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


    void Damage(float damage, Collider2D other, int force)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        if(health <= 0){
            rb2d.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            movementDirection *= -1.0f;
        }
        if(other.gameObject.CompareTag("Sword"))
        {
            Damage(dam, other.GetComponentInParent<Collider2D>(), hitForce);
        }
    }
    void FixedUpdate()
    {
        Movement();


    }
    // Update is called once per frame
    void Update () {
        transform.rotation = Quaternion.identity;

    }
}
