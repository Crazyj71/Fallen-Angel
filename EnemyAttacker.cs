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
    public GameObject player;
    public GameObject sword;
    public bool coroutineOn;
    public Image healthbar;

    private float initHealth;
    private bool hit;
    private Animator anim;
    private Rigidbody2D rb2d;
    float movementDirection = 1.0f;
    private GameObject death;
    // Use this for initialization

    public AudioSource slash1;
    public AudioSource slash2;
    public AudioSource slash3;
    public AudioSource slash4;
    public AudioSource slashFinal;
    public AudioSource vocal1;
    public AudioSource swoosh;


    void PlaySlash()
    {
        int a = Random.Range(0, 3);

        if (a == 0)
        {
            slash1.Play();
        }
        else if (a == 1)
        {
            slash2.Play();
        }
        else if (a == 2)
        {
            slash3.Play();
        }
        else slash4.Play();
    }

    void Start()
    {
        death = GameObject.Find("DeathAnimation");
        death.SetActive(false);
        attacking = false;
        rb2d = GetComponent<Rigidbody2D>();
        sword.SetActive(false);
        coroutineOn = false;
        anim = GetComponentInParent<Animator>();
        anim.SetBool("attacking", false);
        initHealth = health;

    }

    IEnumerator AttackDelay()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        coroutineOn = true;
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(.75f);
        sword.SetActive(true);
        swoosh.Play();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttacker"))
        {
            movementDirection *= -1.0f;
        }
        else if (collision.gameObject.CompareTag("Lava"))
        {
            rb2d.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("EnemyWalker"))
        {
            movementDirection *= -1.0f;
        }
    }

    void Damage(float damage, Collider2D other, int force)
    {

        Vector3 dir = other.transform.position - transform.position;
        dir = -dir.normalized;
        rb2d.AddForce(dir * force);
        health -= damage;
        healthbar.fillAmount = health / initHealth;
        if (health / initHealth < .5)
        {
            healthbar.color = Color.yellow;
        }
        if (health / initHealth < .25)
        {
            healthbar.color = Color.red;
        }

        if (health <= 0)
        {
            slashFinal.Play();
            death.SetActive(true);
            death.transform.position = transform.position;
            AddScore(500);
            rb2d.gameObject.SetActive(false);

        }
        else
        {
            PlaySlash();
        }

        StartCoroutine(TakeDamageColor());
    }

    IEnumerator TakeDamageColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

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
        if (other.gameObject.CompareTag("Boulder"))
        {
            Damage(100, other.GetComponentInParent<Collider2D>(), hitForce);

        }
        if (other.gameObject.CompareTag("Blast"))
        {
            Damage(punchDamage, other.GetComponentInParent<Collider2D>(), hitForce);

        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (rb2d.transform.position.x - other.transform.position.x < 4f
                && rb2d.transform.position.x - other.transform.position.x >-4f)
            {
                Vector3 dir = other.transform.position - transform.position;
                dir = dir.normalized;
                if ((dir.x < 0 && movementDirection > 0) || (dir.x > 0 && movementDirection < 0)) ChangeDirection();
                attacking = true;
            }
        }
    }
    void AddScore(int points)
    {
        PlayerPrefs.SetInt("CurrentScore", PlayerPrefs.GetInt("CurrentScore") + points);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
       /*if (other.gameObject.CompareTag("Player"))
        {
            if (rb2d.transform.position.x - other.transform.position.x < 2
               && rb2d.transform.position.x - other.transform.position.x > -2f)
            {
                Vector3 dir = other.transform.position - transform.position;
                dir = dir.normalized;
                if ((dir.x < 0 && movementDirection > 0) || (dir.x > 0 && movementDirection < 0)) ChangeDirection();
                attacking = true;
            }
        }*/
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attacking = false;
        }
        
    }


    bool playedSound = false;

    void FixedUpdate()
    {
        Movement();
        if (rb2d.transform.position.x - player.transform.position.x < 4f
                && rb2d.transform.position.x - player.transform.position.x > -4f && rb2d.transform.position.y - player.transform.position.y < 4f && rb2d.transform.position.y - player.transform.position.y > -4f)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir = dir.normalized;
            if ((dir.x < 0 && movementDirection > 0) || (dir.x > 0 && movementDirection < 0)) ChangeDirection();
            attacking = true;
        }
        else
        {
            attacking = false;
        }


        if (rb2d.transform.position.x - player.transform.position.x < 5f
                && rb2d.transform.position.x - player.transform.position.x > -5f && (rb2d.transform.position.y - player.transform.position.y < 5f
                && rb2d.transform.position.y - player.transform.position.y > -5f))
        {
            if (!vocal1.isPlaying && playedSound == false)
            {
                vocal1.Play();
                playedSound = true;
            }
        }
        else
        {
            playedSound = false;
        }
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
