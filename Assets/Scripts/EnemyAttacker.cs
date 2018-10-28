using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttacker : MonoBehaviour
{
    public bool facingRight;
    public bool attacking;
    public float runForce;
    public float maxRunSpeed;
    public float health;
    public float swordDamage;
    public float punchDamage;
    public int hitForce;
    public GameObject sword;
    public bool coroutineOn;
    public Image healthbar;

    private Animator anim;
    private Rigidbody2D rb2d;
    float movementDirection = 1.0f;

    // Use this for initialization
    void Start()
    {
        attacking = false;
        rb2d = GetComponent<Rigidbody2D>();
        sword.SetActive(false);
        coroutineOn = false;
        anim = GetComponentInParent<Animator>();

    }

    IEnumerator AttackDelay()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        coroutineOn = true;
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(.25f);
        sword.SetActive(true);
        yield return new WaitForSeconds(.25f);
        sword.SetActive(false);
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1f);
        coroutineOn = false;
        anim.SetBool("attacking", false);
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
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
            healthbar.fillAmount -= damage / 100;
            if (health <= 0)
            {
                rb2d.gameObject.SetActive(false);
            }

    }

    void ChangeDirection()
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            ChangeDirection();

        }
        if (other.gameObject.CompareTag("Sword"))
        {
            Damage(swordDamage, other.GetComponentInParent<Collider2D>(), hitForce);

        }
        if (other.gameObject.CompareTag("Arm"))
        {
            Damage(punchDamage, other.GetComponentInParent<Collider2D>(), hitForce);

        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (rb2d.transform.position.x - other.transform.position.x < 2f
                && rb2d.transform.position.x - other.transform.position.x >-2f)
            {
                Vector3 dir = other.transform.position - transform.position;
                dir = dir.normalized;
                if ((dir.x < 0 && movementDirection > 0) || (dir.x > 0 && movementDirection < 0)) ChangeDirection();
                attacking = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (rb2d.transform.position.x - other.transform.position.x < 2
               && rb2d.transform.position.x - other.transform.position.x > -2f)
            {
                attacking = true;
            }
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
        if (attacking == true && coroutineOn == false)
        {
            StartCoroutine(AttackDelay());
        }
    }
}
