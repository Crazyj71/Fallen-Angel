using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    public bool facingRight;
    public bool attacking;
    public float runForce;
    public float maxRunSpeed;
    public float health;
    public float dam;
    public int hitForce;
    public GameObject sword;

    private Rigidbody2D rb2d;
    float movementDirection = 1.0f;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sword.SetActive(false);
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1f);
        sword.SetActive(true);
        yield return new WaitForSeconds(.25f);
        sword.SetActive(false);
        yield return new WaitForSeconds(1f);
    }

    void Attack()
    {
        if (attacking == true)
        {
            attacking = false;
            StartCoroutine(AttackDelay());
        }
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
            if (currentspeed > maxRunSpeed * -1)
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
        if (health <= 0)
        {
            rb2d.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            movementDirection *= -1.0f;

            if (movementDirection < 0.0f) facingRight = false;
            if (movementDirection > 0.0f) facingRight = true;

            if (facingRight == false)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }


        }
        if (other.gameObject.CompareTag("Sword"))
        {
            Damage(dam, other.GetComponentInParent<Collider2D>(), hitForce);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            attacking = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attacking = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attacking = false;
        }
    }

    void FixedUpdate()
    {
        Movement();


    }
    // Update is called once per frame
    void Update()
    {


         if(attacking == true)
        {
            StartCoroutine(AttackDelay());
        }

    }
}
